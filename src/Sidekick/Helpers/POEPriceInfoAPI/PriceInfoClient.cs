using Newtonsoft.Json;
using Sidekick.Business.Loggers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEPriceInfoAPI
{
    // TODO remove static and use client with DI
    public static class PriceInfoClient
    {
        public const string PoePricesBaseUrl = "https://www.poeprices.info/api";

        public static async Task<PriceInfo> GetItemPricePrediction(string itemText)
        {
            var encodedItem = EncodeItemToBase64(itemText);
            var league = Legacy.TradeClient.SelectedLeague.Id;
            var requestUrl = GenerateRequestUrl(encodedItem, league);

            try
            {
                var response = await Legacy.HttpClientProvider.HttpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var priceInfo = JsonConvert.DeserializeObject<PriceInfo>(result, Legacy.TradeClient.JsonSerializerSettings);
                    priceInfo.ItemText = itemText;
                    return priceInfo;
                }
            }
            catch
            {
                Legacy.Logger.Log("Error getting price prediction from item: " + itemText, LogState.Error);
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

    }
}
