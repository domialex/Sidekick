using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Http;

namespace Sidekick.Business.Apis.PoeNinja
{
    /// <summary>
    /// https://poe.ninja/swagger
    /// </summary>
    public class PoeNinjaClient : IPoeNinjaClient
    {
        private readonly static Uri POE_NINJA_API_BASE_URL = new Uri("https://poe.ninja/api/data/");
        /// <summary>
        /// Poe.ninja uses its own language codes.
        /// </summary>
        public readonly static Dictionary<string, string> POE_NINJA_LANGUAGE_CODES = new Dictionary<string, string>()
        {
             { "de", "ge" }, // German.
             { "en", "en" }, // English.
             { "es", "es" }, // Spanish.
             { "fr", "fr" }, // French.
             { "kr", "ko" }, // Korean.
             { "pt", "pt" }, // Portuguese.
             { "ru", "ru" }, // Russian.
             { "th", "th" }, // Thai.
        };
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public bool IsSupportingCurrentLanguage { get; set; }
        public string LanguageCode { get; set; }

        public PoeNinjaClient(IHttpClientProvider httpClientProvider,
                              ILogger<PoeNinjaClient> logger)
        {
            httpClient = httpClientProvider.HttpClient;
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
            var url = new Uri($"{POE_NINJA_API_BASE_URL}itemoverview?league={leagueId}&type={itemType}&language={LanguageCode}");

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, _jsonSerializerOptions);
            }
            catch (Exception)
            {
                logger.LogInformation("Could not fetch {itemType} from poe.ninja", itemType);
            }

            return new PoeNinjaQueryResult<PoeNinjaItem>() { Lines = new List<PoeNinjaItem>() };
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency)
        {
            var url = new Uri($"{POE_NINJA_API_BASE_URL}currencyoverview?league={leagueId}&type={currency}&language={LanguageCode}");

            try
            {
                var response = await httpClient.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaCurrency>>(responseStream, _jsonSerializerOptions);
            }
            catch
            {
                logger.LogInformation("Could not fetch {currency} from poe.ninja", currency);
            }

            return new PoeNinjaQueryResult<PoeNinjaCurrency>() { Lines = new List<PoeNinjaCurrency>() };
        }
    }
}
