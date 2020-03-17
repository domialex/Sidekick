using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Http;

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

            this.logger = logger.ForContext(GetType());

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
            var url = $"{POE_NINJA_API_BASE_URL}itemoverview?league={leagueId}&type={itemType}";

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, _jsonSerializerOptions);
            }
            catch (Exception)
            {
                logger.Information("Could not fetch {itemType} from poe.ninja", itemType);
            }

            return new PoeNinjaQueryResult<PoeNinjaItem>() { Lines = new System.Collections.Generic.List<PoeNinjaItem>() };
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency)
        {
            var url = $"{POE_NINJA_API_BASE_URL}currencyoverview?league={leagueId}&type={currency}";

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaCurrency>>(responseStream, _jsonSerializerOptions);
            }
            catch
            {
                logger.Information("Could not fetch {currency} from poe.ninja", currency);
            }

            return new PoeNinjaQueryResult<PoeNinjaCurrency>() { Lines = new System.Collections.Generic.List<PoeNinjaCurrency>() };
        }
    }
}
