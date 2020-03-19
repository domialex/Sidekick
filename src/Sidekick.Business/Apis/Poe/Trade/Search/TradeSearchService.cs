using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.Poe.Trade.Search.Requests;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public class TradeSearchService : ITradeSearchService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IHttpClientProvider httpClientProvider;
        private readonly IStaticDataService staticDataService;
        private readonly SidekickSettings configuration;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly INativeBrowser nativeBrowser;

        public TradeSearchService(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientProvider httpClientProvider,
            IStaticDataService staticDataService,
            SidekickSettings configuration,
            IPoeTradeClient poeTradeClient,
            INativeBrowser nativeBrowser)
        {
            this.logger = logger.ForContext(GetType());
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.staticDataService = staticDataService;
            this.configuration = configuration;
            this.poeTradeClient = poeTradeClient;
            this.nativeBrowser = nativeBrowser;
        }

        private async Task<FetchResult<string>> Query(ParsedItem item, List<StatFilter> stats)
        {
            logger.Information("Querying Trade API.");
            FetchResult<string> result = null;

            try
            {
                // TODO: More complex logic for determining bulk vs regular search
                // Maybe also add Fragments to bulk search
                var path = "";
                var json = "";
                string baseUri = null;

                if (item.Rarity == Rarity.Currency)
                {
                    path = $"exchange/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new BulkQueryRequest(item, staticDataService), poeTradeClient.Options);
                    baseUri = languageProvider.Language.PoeTradeExchangeBaseUrl + configuration.LeagueId;
                }
                else
                {
                    path = $"search/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new QueryRequest(item, stats), poeTradeClient.Options);
                    baseUri = languageProvider.Language.PoeTradeSearchBaseUrl + configuration.LeagueId;
                }

                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClientProvider.HttpClient.PostAsync(languageProvider.Language.PoeTradeApiBaseUrl + path, body);

                if (response.IsSuccessStatusCode)
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var content = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<FetchResult<string>>(content, poeTradeClient.Options);

                    result.Uri = new Uri($"{baseUri}/{result.Id}");
                }
                else
                {
                    var responseMessage = await response?.Content?.ReadAsStringAsync();
                    logger.Error("Querying failed: {responseCode} {responseMessage}", response.StatusCode, responseMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception thrown while querying trade api.");
                return null;
            }

            return result;

        }

        public async Task<FetchResult<Result>> GetListingsForSubsequentPages(ParsedItem item, int nextPageToFetch, List<StatFilter> stats = null)
        {
            var queryResult = await Query(item, stats);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(nextPageToFetch, 1).Select(x => GetListings(queryResult, x)));

                return new FetchResult<Result>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        public async Task<FetchResult<Result>> GetListings(ParsedItem item, List<StatFilter> stats = null)
        {
            var queryResult = await Query(item, stats);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(0, 2).Select(x => GetListings(queryResult, x)));

                return new FetchResult<Result>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        private async Task<FetchResult<Result>> GetListings(FetchResult<string> queryResult, int page = 0)
        {
            logger.Information($"Fetching Trade API Listings from Query {queryResult.Id} page {page + 1}.");
            FetchResult<Result> result = null;

            try
            {
                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", queryResult.Result.Skip(page * 10).Take(10)) + "?query=" + queryResult.Id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<FetchResult<Result>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception thrown when fetching trade API listings from Query {queryId} page {page}.", queryResult.Id, page + 1);
                return null;
            }

            return result;
        }

        public async Task OpenWebpage(ParsedItem item)
        {
            var queryResult = await Query(item, null);
            nativeBrowser.Open(queryResult.Uri);
        }
    }
}
