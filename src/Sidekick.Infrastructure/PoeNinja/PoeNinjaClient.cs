using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Languages;
using Sidekick.Infrastructure.PoeNinja.Models;

namespace Sidekick.Infrastructure.PoeNinja
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
        private readonly HttpClient client;
        private readonly ILogger logger;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly JsonSerializerOptions options;

        public string LanguageCode
        {
            get
            {
                if (POE_NINJA_LANGUAGE_CODES.TryGetValue(gameLanguageProvider.Current.LanguageCode, out var languageCode))
                {
                    return languageCode;
                }
                return string.Empty;
            }
        }

        public bool IsSupportingCurrentLanguage => !string.IsNullOrEmpty(LanguageCode);

        public PoeNinjaClient(
            IHttpClientFactory httpClientFactory,
            ILogger<PoeNinjaClient> logger,
            IGameLanguageProvider gameLanguageProvider)
        {
            this.logger = logger;
            this.gameLanguageProvider = gameLanguageProvider;

            options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("Sidekick");
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaItem>> QueryItem(string leagueId, ItemType itemType)
        {
            var url = new Uri($"{POE_NINJA_API_BASE_URL}itemoverview?league={leagueId}&type={itemType}&language={LanguageCode}");

            try
            {
                var response = await client.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, options);
                return result;
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
                var response = await client.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaCurrency>>(responseStream, options);
            }
            catch
            {
                logger.LogInformation("Could not fetch {currency} from poe.ninja", currency);
            }

            return new PoeNinjaQueryResult<PoeNinjaCurrency>() { Lines = new List<PoeNinjaCurrency>() };
        }
    }
}
