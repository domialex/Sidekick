using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Http;
using Sidekick.Core.Loggers;

namespace Sidekick.Business.Apis.PoeNinja
{
    public class PoeNinjaClient : IPoeNinjaClient
    {
        private readonly static Uri POE_NINJA_API_BASE_URL = new Uri("https://poe.ninja/api/data/");
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private JsonSerializerOptions _jsonSerializerOptions;

        public PoeNinjaClient(IHttpClientProvider httpClientProvider, ILogger logger)
        {
            httpClient = httpClientProvider.HttpClient;
            httpClient.BaseAddress = POE_NINJA_API_BASE_URL;

            this.logger = logger;

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaItem>> QueryItem(string leagueId, ItemType itemType)
        {
            var url = $"itemoverview?league={leagueId}&type={itemType}";

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, _jsonSerializerOptions);
            }
            catch
            {
                logger.Log($"Could not fetch {itemType} from poe.ninja");
            }

            return null;
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency)
        {
            var url = $"currencyoverview?league={leagueId}&type={currency}";

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaCurrency>>(responseStream, _jsonSerializerOptions);
            }
            catch
            {
                logger.Log($"Could not fetch {currency} from poe.ninja");
            }

            return null;
        }
    }
}
