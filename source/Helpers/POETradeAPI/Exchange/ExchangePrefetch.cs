using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POETradeAPI.Exchange
{
    class ExchangePrefetch
    {
        public static async Task<ExchangePrefetchResponse> MakeRequest(HttpClient http, Item item, int minimumStock)
        {
            var itemName = await TranslateItemName(item);
            Logger.Log($"ExchangePrefetch: {item.Name}, api name: {itemName}");
            if (itemName == null)
            {
                Logger.Log("ExchangePrefetch: not available for this item");
                return null;
            }

            var options = new Dictionary<string, object>
            {
                { "status", new Dictionary<string, string > { { "option", "online" } } },
                { "have", new List<string> { "chaos" } },
                { "want",  new List<string> { itemName } },
                { "minimum", minimumStock }
            };
            var body = new Dictionary<string, object> { { "exchange", options } };
            var bodySerialized = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            // TODO: league is currently hardcoded in URL
            var response = await http.PostAsync("https://www.pathofexile.com/api/trade/exchange/Metamorph", bodySerialized);
            Logger.Log($"ExchangePrefetch response status: {response.StatusCode}");
            if (!response.IsSuccessStatusCode) return null;

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangePrefetchResponse>(responseText);
        }

        private static IDictionary<string, string> _nameToIdMap = null;

        /// <summary>Translates an item's full name into the id used by the exchange API.
        /// For instance, "Orb of Fusing" is converted to "fuse".</summary>
        private static async Task<string> TranslateItemName(Item item)
        {
            if (_nameToIdMap == null)
            {
                var uri = new Uri("Resources/ExchangeItemIds.json", UriKind.Relative);
                var resource = System.Windows.Application.GetContentStream(uri);
                using (var reader = new StreamReader(resource.Stream))
                {
                    var text = await reader.ReadToEndAsync();
                    _nameToIdMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                }
            }

            return _nameToIdMap[item.Name];
        }        
    }

    public partial class ExchangePrefetchResponse
    {
        [JsonProperty("result")]
        public string[] Result { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}
