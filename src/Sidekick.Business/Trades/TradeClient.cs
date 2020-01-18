using Newtonsoft.Json;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Leagues;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Trades.Models;
using Sidekick.Business.Trades.Requests;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    [SidekickService(typeof(ITradeClient))]
    public class TradeClient : ITradeClient, IOnBeforeInitialize, IOnReset
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IHttpClientProvider httpClientProvider;
        private readonly IPoeApiService poeApiService;
        private readonly ILeagueService leagueService;

        public TradeClient(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientProvider httpClientProvider,
            IPoeApiService poeApiService,
            ILeagueService leagueService)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.poeApiService = poeApiService;
            this.leagueService = leagueService;
        }

        public List<StaticItemCategory> StaticItemCategories { get; private set; }

        public List<AttributeCategory> AttributeCategories { get; private set; }

        public List<ItemCategory> ItemCategories { get; private set; }

        public HashSet<string> MapNames { get; private set; }

        public async Task OnBeforeInitialize()
        {
            logger.Log("Fetching Path of Exile trade data.");

            var fetchStaticItemCategoriesTask = poeApiService.Fetch<StaticItemCategory>("Static item categories", "static");
            var fetchAttributeCategoriesTask = poeApiService.Fetch<AttributeCategory>("Attribute categories", "stats");
            var fetchItemCategoriesTask = poeApiService.Fetch<ItemCategory>("Item categories", "items");

            StaticItemCategories = await fetchStaticItemCategoriesTask;
            AttributeCategories = await fetchAttributeCategoriesTask;
            ItemCategories = await fetchItemCategoriesTask;

            logger.Log($"Path of Exile trade data fetched.");
        }

        public Task OnInitialize()
        {
            var mapCategories = StaticItemCategories.Where(c => MapTiers.TierIds.Contains(c.Id)).ToList();
            var allMapNames = new List<string>();

            foreach (var item in mapCategories)
            {
                allMapNames.AddRange(item.Entries.Select(c => c.Text));
            }

            MapNames = new HashSet<string>(allMapNames.Distinct());

            return Task.CompletedTask;
        }

        public Task OnReset()
        {
            StaticItemCategories = null;
            AttributeCategories = null;
            ItemCategories = null;

            return Task.CompletedTask;
        }

        public async Task<QueryResult<string>> Query(Parsers.Models.Item item)
        {
            logger.Log("Querying Trade API.");
            QueryResult<string> result = null;

            try
            {
                // TODO: More complex logic for determining bulk vs regular search
                // Maybe also add Fragments to bulk search
                var isBulk = (item.GetType() == typeof(CurrencyItem) || item.GetType() == typeof(DivinationCardItem));

                StringContent body;

                if (isBulk)
                {
                    var bulkQueryRequest = new BulkQueryRequest(item, languageProvider.Language, this);
                    body = new StringContent(JsonConvert.SerializeObject(bulkQueryRequest, httpClientProvider.JsonSerializerSettings), Encoding.UTF8, "application/json");
                }
                else
                {
                    var queryRequest = new QueryRequest(item, languageProvider.Language);
                    body = new StringContent(JsonConvert.SerializeObject(queryRequest, httpClientProvider.JsonSerializerSettings), Encoding.UTF8, "application/json");
                }

                var response = await httpClientProvider.HttpClient.PostAsync(languageProvider.Language.PoeTradeApiBaseUrl + $"{(isBulk ? "exchange" : "search")}/" + leagueService.SelectedLeague.Id, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<QueryResult<string>>(content);

                    var baseUri = isBulk ? languageProvider.Language.PoeTradeExchangeBaseUrl : languageProvider.Language.PoeTradeSearchBaseUrl;
                    result.Uri = new Uri(baseUri + leagueService.SelectedLeague.Id + "/" + result.Id);
                }
            }
            catch
            {
                return null;
            }

            return result;

        }

        public async Task<QueryResult<ListingResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch)
        {
            var queryResult = await Query(item);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(nextPageToFetch, 1).Select(x => GetListings(queryResult, x)));

                return new QueryResult<ListingResult>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Item = item,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        public async Task<QueryResult<ListingResult>> GetListings(Parsers.Models.Item item)
        {
            var queryResult = await Query(item);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(0, 1).Select(x => GetListings(queryResult, x)));

                return new QueryResult<ListingResult>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Item = item,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        public async Task<QueryResult<ListingResult>> GetListings(QueryResult<string> queryResult, int page = 0)
        {
            logger.Log($"Fetching Trade API Listings from Query {queryResult.Id} page {(page + 1).ToString()}.");
            QueryResult<ListingResult> result = null;

            try
            {
                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", queryResult.Result.Skip(page * 10).Take(10)) + "?query=" + queryResult.Id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<QueryResult<ListingResult>>(content);
                }
            }
            catch
            {
                return null;
            }


            return result;
        }

        public async Task OpenWebpage(Parsers.Models.Item item)
        {
            var queryResult = await Query(item);

            var uri = queryResult.Uri.ToString();
            logger.Log($"Opening in browser: {uri}");
            Process.Start(uri);
        }
    }
}
