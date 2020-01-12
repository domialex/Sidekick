using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEPriceInfoAPI
{
    // TODO remove static and use client with DI
    public static class PriceInfoClient
    {
        public const string PoePricesBaseUrl = "https://www.poeprices.info/api";
        public static League SelectedLeague;

        private static HttpClient _client;
        private static JsonSerializerSettings _jsonSerializerSettings;

        public static void Initialize()
        {
            _client = new HttpClient();

            if (_jsonSerializerSettings == null)
            {
                _jsonSerializerSettings = new JsonSerializerSettings();
                _jsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
        }

        public static async Task<PriceInfo> GetItemPricePrediction(string itemText)
        {
            var encodedItem = EncodeItemToBase64(itemText);
            var league = SelectedLeague.Id;
            var requestUrl = GenerateRequestUrl(encodedItem, league);

            try
            {
                var response = await _client.GetAsync(requestUrl);

                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var priceInfo = JsonConvert.DeserializeObject<PriceInfo>(result, _jsonSerializerSettings);
                    priceInfo.ItemText = itemText;
                    return priceInfo;
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Error getting price prediction from item: " + itemText, LogState.Error);
            }

            return null;
        }

        private static string EncodeItemToBase64(string itemText)
        {
            var bytes = Encoding.UTF8.GetBytes(itemText);
            return Convert.ToBase64String(bytes);
        }

        private static string GenerateRequestUrl(string encodedItem, string selectedLeague)
        {
            return PoePricesBaseUrl + "?l=" + selectedLeague + "&i=" + encodedItem;
        }

        public static void Dispose()
        {
            _client.Dispose();
        }
    }
}
