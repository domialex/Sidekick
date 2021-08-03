using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Common.Settings;
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
        private readonly ISettings settings;
        private readonly JsonSerializerOptions options;

        private string LanguageCode
        {
            get
            {
                if (POE_NINJA_LANGUAGE_CODES.TryGetValue(gameLanguageProvider.Language.LanguageCode, out var languageCode))
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
            IGameLanguageProvider gameLanguageProvider,
            ISettings settings)
        {
            this.logger = logger;
            this.gameLanguageProvider = gameLanguageProvider;
            this.settings = settings;
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

        public async Task<PoeNinjaQueryResult<PoeNinjaItem>> FetchItems(ItemType itemType)
        {
            var url = new Uri($"{POE_NINJA_API_BASE_URL}itemoverview?league={settings.LeagueId}&type={itemType}&language={LanguageCode}");

            try
            {
                var response = await client.GetAsync(url);
                var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<PoeNinjaQueryResult<PoeNinjaItem>>(responseStream, options);
            }
            catch (Exception)
            {
                logger.LogInformation("Could not fetch {itemType} from poe.ninja", itemType);
            }

            return new PoeNinjaQueryResult<PoeNinjaItem>() { Lines = new List<PoeNinjaItem>() };
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> FetchCurrencies(CurrencyType currency)
        {
            var url = new Uri($"{POE_NINJA_API_BASE_URL}currencyoverview?league={settings.LeagueId}&type={currency}&language={LanguageCode}");

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
