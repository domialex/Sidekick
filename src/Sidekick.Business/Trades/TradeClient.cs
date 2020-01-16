using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Loggers;
using Sidekick.Business.Notifications;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Trades.Models;
using Sidekick.Business.Trades.Requests;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    [SidekickService(typeof(ITradeClient))]
    public class TradeClient : ITradeClient
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IHttpClientProvider httpClientProvider;
        private readonly INotificationService notificationService;

        public TradeClient(ILogger logger, ILanguageProvider languageProvider, IHttpClientProvider httpClientProvider, INotificationService notificationService)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
            this.notificationService = notificationService;

            languageProvider.LanguageChanged += LanguageProvider_LanguageChanged;

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        private async Task LanguageProvider_LanguageChanged()
        {
            Dispose();
            await Initialize();
        }

        public JsonSerializerSettings JsonSerializerSettings { get; private set; }
        private bool IsFetching { get; set; }
        private bool OneFetchFailed { get; set; }

        private List<League> _leagues = null;
        public List<League> Leagues
        {
            get => _leagues;
            set { _leagues = value; LeaguesChanged?.Invoke(null, null); }
        }
        public event EventHandler LeaguesChanged;

        public List<StaticItemCategory> StaticItemCategories { get; private set; }

        public List<AttributeCategory> AttributeCategories { get; private set; }

        public List<ItemCategory> ItemCategories { get; private set; }

        public HashSet<string> MapNames { get; private set; }

        public bool IsReady { get; private set; }

        public League SelectedLeague { get; set; }

        public async Task<bool> Initialize()
        {
            if (IsFetching)
            {
                return false;
            }

            IsFetching = true;
            logger.Log("Fetching Path of Exile trade data.");

            await FetchAPIData();

            if (OneFetchFailed)
            {
                logger.Log("Retrying every minute.");
                Dispose();
                await Task.Run(Retry);
                return false;
            }

            IsFetching = false;

            IsReady = true;

            logger.Log($"Path of Exile trade data fetched.");
            logger.Log($"Sidekick is ready, press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay.");

            notificationService.NotifyTray(new Notification()
            {
                Title = "Sidekick is ready",
                Text = $"Press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay."
            });

            return true;
        }

        private async void Retry()
        {
            await Task.Delay(TimeSpan.FromMinutes(1));
            if (IsFetching)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                Retry();
            }
            else
            {
                await Initialize();
            }
        }

        public async Task FetchAPIData()
        {
            var fetchLeaguesTask = FetchDataAsync<League>("Leagues", "leagues");
            var fetchStaticItemCategoriesTask = FetchDataAsync<StaticItemCategory>("Static item categories", "static");
            var fetchAttributeCategoriesTask = FetchDataAsync<AttributeCategory>("Attribute categories", "stats");
            var fetchItemCategoriesTask = FetchDataAsync<ItemCategory>("Item categories", "items");

            Leagues = await fetchLeaguesTask;
            StaticItemCategories = await fetchStaticItemCategoriesTask;
            AttributeCategories = await fetchAttributeCategoriesTask;
            ItemCategories = await fetchItemCategoriesTask;

            if (OneFetchFailed)
            {
                return;
            }

            var mapCategories = StaticItemCategories.Where(c => MapTiers.TierIds.Contains(c.Id)).ToList();
            var allMapNames = new List<string>();

            foreach (var item in mapCategories)
            {
                allMapNames.AddRange(item.Entries.Select(c => c.Text));
            }

            MapNames = new HashSet<string>(allMapNames.Distinct());
        }

        private async Task<List<T>> FetchDataAsync<T>(string name, string path) where T : class
        {
            logger.Log($"Fetching {name}.".PadLeft(4));
            List<T> result = null;

            try
            {
                var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "data/" + path);
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<QueryResult<T>>(content, JsonSerializerSettings)?.Result;
                logger.Log($"{result.Count.ToString().PadRight(3)} {name} fetched.");
            }
            catch
            {
                OneFetchFailed = true;
                logger.Log($"Could not fetch {name}.");
            }

            return result;
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
                    body = new StringContent(JsonConvert.SerializeObject(bulkQueryRequest, JsonSerializerSettings), Encoding.UTF8, "application/json");
                }
                else
                {
                    var queryRequest = new QueryRequest(item, languageProvider.Language);
                    body = new StringContent(JsonConvert.SerializeObject(queryRequest, JsonSerializerSettings), Encoding.UTF8, "application/json");
                }

                var response = await httpClientProvider.HttpClient.PostAsync(languageProvider.Language.PoeTradeApiBaseUrl + $"{(isBulk ? "exchange" : "search")}/" + SelectedLeague.Id, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<QueryResult<string>>(content);

                    var baseUri = isBulk ? languageProvider.Language.PoeTradeExchangeBaseUrl : languageProvider.Language.PoeTradeSearchBaseUrl;
                    result.Uri = new Uri(baseUri + SelectedLeague.Id + "/" + result.Id);
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

        public void Dispose()
        {
            Leagues = null;
            StaticItemCategories = null;
            AttributeCategories = null;
            ItemCategories = null;

            IsReady = false;
            IsFetching = false;
            OneFetchFailed = false;
        }
    }
}
