using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.POETradeAPI.Models;
using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POETradeAPI
{
    public interface ITradeClient : IDisposable
    {
        List<League> Leagues { get; }
        List<StaticItemCategory> StaticItemCategories { get; }
        List<AttributeCategory> AttributeCategories { get; }
        List<ItemCategory> ItemCategories { get; }
        HashSet<string> MapNames { get; }
        bool IsReady { get; }
        League SelectedLeague { get; set; }

        Task Initialize();
        Task FetchAPIData();
        Task<QueryResult<string>> Query(Item item);
        Task<QueryResult<ListingResult>> GetListings(Item item);
        Task<QueryResult<ListingResult>> GetListings(QueryResult<string> queryResult, int page = 0);
    }

    public class DefaultTradeClient : ITradeClient
    {
        public List<League> Leagues { get; private set; }
        public List<StaticItemCategory> StaticItemCategories { get; private set; }
        public List<AttributeCategory> AttributeCategories { get; private set; }
        public List<ItemCategory> ItemCategories { get; private set; }
        public HashSet<string> MapNames { get; private set; }
        public bool IsReady { get; private set; }
        public League SelectedLeague { get; set; }

        private JsonSerializerSettings _jsonSerializerSettings;
        private HttpClient _httpClient;
        private bool IsFetching;
        private bool OneFetchFailed;

        public async Task Initialize()
        {
            if (_jsonSerializerSettings == null)
            {
                _jsonSerializerSettings = new JsonSerializerSettings();
                _jsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }

            if (IsFetching)
            {
                return;
            }

            IsFetching = true;
            Logger.Log("Fetching Path of Exile trade data.");

            _httpClient = new HttpClient();

            await FetchAPIData();

            if (OneFetchFailed)
            {
                Logger.Log("Retrying every minute.");
                Dispose();
                await Task.Run(Retry);
                return;
            }

            IsFetching = false;

            TrayIcon.PopulateLeagueSelectMenu(Leagues);

            IsReady = true;

            Logger.Log($"Path of Exile trade data fetched.");
            Logger.Log($"Sidekick is ready, press Ctrl+D over an item in-game to use. Press Escape to close overlay.");
            TrayIcon.SendNotification("Press Ctrl+D over an item in-game to use. Press Escape to close overlay.", "Sidekick is ready");
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
                Initialize();
            }
        }

        public async Task FetchAPIData()
        {
            Leagues = await FetchDataAsync<League>("Leagues", "leagues");
            StaticItemCategories = await FetchDataAsync<StaticItemCategory>("Static item categories", "static");
            AttributeCategories = await FetchDataAsync<AttributeCategory>("Attribute categories", "stats");
            ItemCategories = await FetchDataAsync<ItemCategory>("Item categories", "items");
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
            Logger.Log($"Fetching {name}.".PadLeft(4));
            List<T> result = null;

            try
            {
                var response = await _httpClient.GetAsync(LanguageSettings.Provider.PoeTradeApiBaseUrl + "data/" + path);
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<QueryResult<T>>(content, _jsonSerializerSettings)?.Result;
                Logger.Log($"{result.Count.ToString().PadRight(3)} {name} fetched.");
            }
            catch
            {
                OneFetchFailed = true;
                Logger.Log($"Could not fetch {name}.");
            }

            return result;
        }

        public async Task<QueryResult<string>> Query(Item item)
        {
            Logger.Log("Querying Trade API.");
            QueryResult<string> result = null;

            try
            {
                // TODO: More complex logic for determining bulk vs regular search
                // Maybe also add Fragments to bulk search
                var isBulk = (item.GetType() == typeof(CurrencyItem) || item.GetType() == typeof(DivinationCardItem));

                StringContent body;
                if (isBulk)
                {
                    var bulkQueryRequest = new BulkQueryRequest(item);
                    body = new StringContent(JsonConvert.SerializeObject(bulkQueryRequest, _jsonSerializerSettings), Encoding.UTF8, "application/json");
                }
                else
                {
                    var queryRequest = new QueryRequest(item);
                    body = new StringContent(JsonConvert.SerializeObject(queryRequest, _jsonSerializerSettings), Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.PostAsync(LanguageSettings.Provider.PoeTradeApiBaseUrl + $"{(isBulk ? "exchange" : "search")}/" + SelectedLeague.Id, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<QueryResult<string>>(content);

                    var baseUri = isBulk ? LanguageSettings.Provider.PoeTradeExchangeBaseUrl : LanguageSettings.Provider.PoeTradeSearchBaseUrl;
                    result.Uri = baseUri + SelectedLeague.Id + "/" + result.Id;
                }
            }
            catch
            {
                return null;
            }

            return result;
        }

        public async Task<QueryResult<ListingResult>> GetListings(Item item)
        {
            var queryResult = await Query(item);
            if (queryResult != null)
            {
                var result = await Task.WhenAll(Enumerable.Range(0, 2).Select(x => GetListings(queryResult, x)));

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
            Logger.Log($"Fetching Trade API Listings from Query {queryResult.Id} page {(page + 1).ToString()}.");
            QueryResult<ListingResult> result = null;

            try
            {
                var response = await _httpClient.GetAsync(LanguageSettings.Provider.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", queryResult.Result.Skip(page * 10).Take(10)) + "?query=" + queryResult.Id);
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
            _httpClient.Dispose();
            _httpClient = null;

            Leagues = null;
            StaticItemCategories = null;
            AttributeCategories = null;
            ItemCategories = null;

            IsReady = false;
            IsFetching = false;
            OneFetchFailed = false;
        }
    }

    public static class TradeClient
    {
        public static ITradeClient Client = new DefaultTradeClient();

        public static List<League> Leagues => Client.Leagues;
        public static List<StaticItemCategory> StaticItemCategories => Client.StaticItemCategories;
        public static List<AttributeCategory> AttributeCategories => Client.AttributeCategories;
        public static List<ItemCategory> ItemCategories => Client.ItemCategories;
        public static HashSet<string> MapNames => Client.MapNames;
        public static bool IsReady => Client.IsReady;
        public static League SelectedLeague { get { return Client.SelectedLeague; } set { Client.SelectedLeague = value; } }

        public static async Task Initialize()
        {
            await Client.Initialize();
        }

        public static async Task FetchAPIData()
        {
            await Client.FetchAPIData();
        }

        public static async Task<QueryResult<string>> Query(Item item)
        {
            return await Client.Query(item);
        }

        public static async Task<QueryResult<ListingResult>> GetListings(Item item)
        {
            return await Client.GetListings(item);
        }

        public static async Task<QueryResult<ListingResult>> GetListings(QueryResult<string> queryResult, int page = 0)
        {
            return await Client.GetListings(queryResult, page);
        }

        public static void Dispose()
        {
            Client.Dispose();
        }
    }
}
