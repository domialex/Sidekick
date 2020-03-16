using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Trades.Requests;
using Sidekick.Business.Trades.Results;
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
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.staticDataService = staticDataService;
            this.configuration = configuration;
            this.poeTradeClient = poeTradeClient;
            this.nativeBrowser = nativeBrowser;
        }

        private async Task<FetchResult<string>> Query(Parsers.Models.Item item)
        {
            logger.LogInformation("Querying Trade API.");
            FetchResult<string> result = null;

            try
            {
                // TODO: More complex logic for determining bulk vs regular search
                // Maybe also add Fragments to bulk search
                var path = "";
                var json = "";
                string baseUri = null;

                if (IsBulk(item.Type))
                {
                    path = $"exchange/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new BulkQueryRequest(item, languageProvider.Language, staticDataService), poeTradeClient.Options);
                    baseUri = languageProvider.Language.PoeTradeExchangeBaseUrl + configuration.LeagueId;
                }
                else
                {
                    path = $"search/{configuration.LeagueId}";
                    json = JsonSerializer.Serialize(new QueryRequest(item), poeTradeClient.Options);
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
                    logger.LogError("Querying failed.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Querying error.");
                return null;
            }

            return result;

        }

        public async Task<FetchResult<SearchResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch)
        {
            var queryResult = await Query(item);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(nextPageToFetch, 1).Select(x => GetListings(queryResult, x)));

                return new FetchResult<SearchResult>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        public async Task<FetchResult<SearchResult>> GetListings(Parsers.Models.Item item)
        {
            var queryResult = await Query(item);

            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(0, 2).Select(x => GetListings(queryResult, x)));

                return new FetchResult<SearchResult>()
                {
                    Id = queryResult.Id,
                    Result = result.Where(x => x != null).SelectMany(x => x.Result).ToList(),
                    Total = queryResult.Total,
                    Uri = queryResult.Uri
                };
            }

            return null;
        }

        public async Task<FetchResult<SearchResult>> GetListings(FetchResult<string> queryResult, int page = 0)
        {
            logger.LogInformation($"Fetching Trade API Listings from Query {queryResult.Id} page {page + 1}.");
            FetchResult<SearchResult> result = null;

            try
            {
                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", queryResult.Result.Skip(page * 10).Take(10)) + "?query=" + queryResult.Id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<FetchResult<SearchResult>>(content, new JsonSerializerOptions()
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
            nativeBrowser.Open(queryResult.Uri);
        }

        private bool IsBulk(string type)
        {
            return staticDataService.Lookup.ContainsKey(type);
        }
    }
}
