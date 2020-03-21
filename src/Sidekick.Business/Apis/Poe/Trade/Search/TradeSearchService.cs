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

        public async Task<FetchResult<string>> SearchBulk(ParsedItem item)
        {
            try
            {
                logger.Information("Querying Exchange API.");

                var uri = $"{languageProvider.Language.PoeTradeApiBaseUrl}exchange/{configuration.LeagueId}";
                var json = JsonSerializer.Serialize(new BulkQueryRequest(item, staticDataService), poeTradeClient.Options);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClientProvider.HttpClient.PostAsync(uri, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<FetchResult<string>>(content, poeTradeClient.Options);
                    result.Uri = new Uri($"{languageProvider.Language.PoeTradeSearchBaseUrl}{configuration.LeagueId}/{result.Id}");
                    return result;
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
            }

            return null;
        }

        public async Task<FetchResult<string>> Search(ParsedItem item, SearchFilters filters = null, List<StatFilter> stats = null)
        {
            try
            {
                logger.Information("Querying Trade API.");

                if (filters == null)
                {
                    filters = new SearchFilters();
                }

                var request = new QueryRequest();
                request.Query.Filters = filters;

                // Auto Search 5+ Links
                var highestCount = item.Sockets
                    .GroupBy(x => x.Group)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
                if (highestCount >= 5)
                {
                    request.Query.Filters.SocketFilters.Filters.Links = new SocketFilterOption()
                    {
                        Min = highestCount,
                    };
                }

                if (item.Rarity == Rarity.Unique)
                {
                    request.Query.Name = item.Name;
                    request.Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "Unique",
                    };
                }
                else if (item.Rarity == Rarity.Prophecy)
                {
                    request.Query.Name = item.Name;
                }
                else
                {
                    request.Query.Type = item.TypeLine;
                    request.Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "nonunique",
                    };
                }

                if (item.MapTier > 0)
                {
                    request.Query.Filters.MapFilters.Filters.MapTier = new SearchFilterValue()
                    {
                        Min = item.MapTier,
                        Max = item.MapTier,
                    };
                }

                if (stats != null && stats.Count > 0)
                {
                    request.Query.Stats = new List<StatFilterGroup>()
                    {
                        new StatFilterGroup()
                        {
                            Type = StatType.And,
                            Filters = stats
                        }
                    };
                }

                var uri = $"{languageProvider.Language.PoeTradeApiBaseUrl}search/{configuration.LeagueId}";
                var json = JsonSerializer.Serialize(request, poeTradeClient.Options);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClientProvider.HttpClient.PostAsync(uri, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<FetchResult<string>>(content, poeTradeClient.Options);
                    result.Uri = new Uri($"{languageProvider.Language.PoeTradeSearchBaseUrl}{configuration.LeagueId}/{result.Id}");
                    return result;
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
            }

            return null;
        }

        public async Task<FetchResult<Result>> GetResults(string queryId, List<string> ids)
        {
            try
            {
                logger.Information($"Fetching Trade API Listings from Query {queryId}.");

                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", ids) + "?query=" + queryId);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<FetchResult<Result>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Exception thrown when fetching trade API listings from Query {queryId}.");
            }

            return null;
        }

        public async Task OpenWebpage(ParsedItem item)
        {
            if (item.Rarity == Rarity.Currency)
            {
                nativeBrowser.Open((await SearchBulk(item)).Uri);
            }
            else
            {
                nativeBrowser.Open((await Search(item)).Uri);
            }
        }
    }
}
