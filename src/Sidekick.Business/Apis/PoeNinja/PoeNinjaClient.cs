using Sidekick.Business.Http;
using Sidekick.Business.Apis.PoeNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Sidekick.Business.Apis.PoeNinja
{
    public class PoeNinjaClient : IPoeNinjaClient
    {
        public JsonSerializerOptions Options
        {
            get
            {
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true,
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                return options;
            }
        }

        public readonly static Uri POE_NINJA_ITEMOVERVIEW_BASE_URL = new Uri("https://poe.ninja/api/data/itemoverview");
        public readonly static Uri POE_NINJA_CURRENCYOVERVIEW_BASE_URL = new Uri("https://poe.ninja/api/data/currencyoverview");
        private readonly HttpClient _client;

        public PoeNinjaClient(IHttpClientProvider httpClientProvider)
        {
            _client = httpClientProvider.HttpClient;
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaItem>> GetItemOverview(string league, ItemType itemType)
        {

            var url = $"{POE_NINJA_ITEMOVERVIEW_BASE_URL}?league={league}&type={itemType}";

            var responseStream = await PerformRequestAndValidateResponse(url);

            return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, Options);
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> GetCurrencyOverview(string league, CurrencyType currency)
        {

            var url = $"{POE_NINJA_CURRENCYOVERVIEW_BASE_URL}?league={league}&type={currency}";

            var responseStream = await PerformRequestAndValidateResponse(url);

            return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaCurrency>>(responseStream, Options);
        }

        private async Task<Stream> PerformRequestAndValidateResponse(string url)
        {
            var response = await _client.GetAsync(url);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"poe.ninja returned an error for {url}: [{response.StatusCode}] {responseStream}");
            }

            if (responseStream == null)
            {
                throw new Exception("poe.ninja returned an empty result. Check the provided league.");
            }

            return responseStream;
        }
    }
}
