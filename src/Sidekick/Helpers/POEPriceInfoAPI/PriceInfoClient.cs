using Sidekick.Core.Loggers;
using Sidekick.Helpers.POEPriceInfoAPI.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEPriceInfoAPI
{
    // TODO remove static and use client with DI
    public static class PriceInfoClient
    {
        public const string PoePricesBaseUrl = "https://www.poeprices.info/api";

        public static async Task<PriceInfoResult> GetItemPricePrediction(string itemText)
        {
            var encodedItem = Convert.ToBase64String(Encoding.UTF8.GetBytes(itemText));
            var requestUrl = PoePricesBaseUrl + "?l=" + Legacy.Configuration.LeagueId + "&i=" + encodedItem;

            try
            {
                var response = await Legacy.HttpClientProvider.HttpClient.GetAsync(requestUrl);
                var content = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<PriceInfoResult>(content, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });


            }
            catch
            {
                Legacy.Logger.Log("Error getting price prediction from poeprices.info", LogState.Error);
            }

            return null;
        }
    }
}
