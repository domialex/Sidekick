using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.Poe.Trade.Search.Requests;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;

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
        private readonly IStatDataService statDataService;
        private readonly IMediator mediator;

        public TradeSearchService(ILogger<TradeSearchService> logger,
            ILanguageProvider languageProvider,
            IHttpClientProvider httpClientProvider,
            IStaticDataService staticDataService,
            SidekickSettings configuration,
            IPoeTradeClient poeTradeClient,
            IStatDataService statDataService,
            IMediator mediator)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.staticDataService = staticDataService;
            this.configuration = configuration;
            this.poeTradeClient = poeTradeClient;
            this.statDataService = statDataService;
            this.mediator = mediator;
        }

        public async Task<FetchResult<string>> SearchBulk(Item item)
        {
            try
            {
                logger.LogInformation("Querying Exchange API.");

                var uri = $"{languageProvider.Language.PoeTradeApiBaseUrl}exchange/{configuration.LeagueId}";
                var json = JsonSerializer.Serialize(new BulkQueryRequest(item, staticDataService), poeTradeClient.Options);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClientProvider.HttpClient.PostAsync(uri, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<FetchResult<string>>(content, poeTradeClient.Options);
                    result.Uri = new Uri($"{languageProvider.Language.PoeTradeExchangeBaseUrl}{configuration.LeagueId}/{result.Id}");
                    return result;
                }
                else
                {
                    var responseMessage = await response?.Content?.ReadAsStringAsync();
                    logger.LogError("Querying failed: {responseCode} {responseMessage}", response.StatusCode, responseMessage);
                    logger.LogError("Uri: {uri}", uri);
                    logger.LogError("Query: {query}", json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception thrown while querying trade api.");
            }

            return null;
        }

        public async Task<FetchResult<string>> Search(Item item, SearchFilters filters = null, List<StatFilter> stats = null)
        {
            try
            {
                logger.LogInformation("Querying Trade API.");

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
                    request.Query.Type = item.Type;
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
                    if (string.IsNullOrEmpty(request.Query.Filters.TypeFilters.Filters.Category?.Option))
                    {
                        request.Query.Type = item.Type;
                    }
                    request.Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "nonunique",
                    };
                }

                if (item.Properties.AlternateQuality)
                {
                    request.Query.Term = item.NameLine;
                }

                if (item.Properties.MapTier > 0)
                {
                    request.Query.Filters.MapFilters.Filters.MapTier = new SearchFilterValue()
                    {
                        Min = item.Properties.MapTier,
                        Max = item.Properties.MapTier,
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

                var uri = new Uri($"{languageProvider.Language.PoeTradeApiBaseUrl}search/{configuration.LeagueId}");
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
                    logger.LogError("Querying failed: {responseCode} {responseMessage}", response.StatusCode, responseMessage);
                    logger.LogError("Uri: {uri}", uri);
                    logger.LogError("Query: {query}", json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception thrown while querying trade api.");
            }

            return null;
        }

        public async Task<FetchResult<TradeItem>> GetResults(string queryId, List<string> ids, List<StatFilter> stats = null)
        {
            try
            {
                logger.LogInformation($"Fetching Trade API Listings from Query {queryId}.");

                var pseudo = string.Empty;
                if (stats != null)
                {
                    pseudo = string.Join("", stats.Where(x => x.Id.StartsWith("pseudo.")).Select(x => $"&pseudos[]={x.Id}"));
                }

                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", ids) + "?query=" + queryId + pseudo);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<FetchResult<Result>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });
                    return new FetchResult<TradeItem>()
                    {
                        Id = result.Id,
                        Result = result.Result.Select(x => new TradeItem(x, statDataService)).ToList(),
                        Total = result.Total,
                        Uri = result.Uri,
                    };
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception thrown when fetching trade API listings from Query {queryId}.");
            }

            return null;
        }

        public async Task OpenWebpage(Item item)
        {
            if (item.Rarity == Rarity.Currency)
            {
                await mediator.Send(new OpenBrowserCommand((await SearchBulk(item)).Uri));
            }
            else
            {
                await mediator.Send(new OpenBrowserCommand((await Search(item)).Uri));
            }
        }
    }
}
