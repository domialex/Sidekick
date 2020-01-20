using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Categories;
using Sidekick.Business.Http;
using Sidekick.Business.Languages.Client;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Trades.Requests;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Configuration;
using Sidekick.Core.Loggers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    public class TradeClient : ITradeClient
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IHttpClientProvider httpClientProvider;
        private readonly IStaticItemCategoryService staticItemCategoryService;
        private readonly Configuration configuration;
        private readonly IPoeApiService poeApiService;

        public TradeClient(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientProvider httpClientProvider,
            IStaticItemCategoryService staticItemCategoryService,
            Configuration configuration,
            IPoeApiService poeApiService)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.staticItemCategoryService = staticItemCategoryService;
            this.configuration = configuration;
            this.poeApiService = poeApiService;
        }

        private async Task<QueryResult<string>> Query(Parsers.Models.Item item)
        {
            logger.Log("Querying Trade API.");
            QueryResult<string> result = null;

            try
            {
                // TODO: More complex logic for determining bulk vs regular search
                // Maybe also add Fragments to bulk search
                string path = "";
                string json = "";
                string baseUri = null;
                if (item is CurrencyItem || item is DivinationCardItem)
                {
                    path = $"exchange/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new BulkQueryRequest(item, languageProvider.Language, staticItemCategoryService), poeApiService.Options);
                    baseUri = languageProvider.Language.PoeTradeExchangeBaseUrl + configuration.LeagueId;
                }
                else
                {
                    path = $"search/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new QueryRequest(item, languageProvider.Language), poeApiService.Options);
                    baseUri = languageProvider.Language.PoeTradeSearchBaseUrl + configuration.LeagueId;
                }

                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClientProvider.HttpClient.PostAsync(languageProvider.Language.PoeTradeApiBaseUrl + path, body);

                if (response.IsSuccessStatusCode)
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var content = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<QueryResult<string>>(content, poeApiService.Options);

                    result.Uri = new Uri($"{baseUri}/{result.Id}");
                }
                else
                {
                    logger.Log("Querying failed.");
                }
            }
            catch (Exception e)
            {
                logger.Log("Querying error.");
                logger.LogException(e);
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
            logger.Log($"Fetching Trade API Listings from Query {queryResult.Id} page {page + 1}.");
            QueryResult<ListingResult> result = null;

            try
            {
                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", queryResult.Result.Skip(page * 10).Take(10)) + "?query=" + queryResult.Id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<QueryResult<ListingResult>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });
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
