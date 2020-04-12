using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.PoeNinja
{
    /// <summary>
    /// https://poe.ninja/swagger
    /// </summary>
    public class PoeNinjaClient : IPoeNinjaClient, IOnAfterInit
    {
        private readonly static Uri POE_NINJA_API_BASE_URL = new Uri("https://poe.ninja/api/data/");
        /// <summary>
        /// Poe.ninja uses its own language codes.
        /// </summary>
        private readonly static Dictionary<string, string> POE_NINJA_LANGUAGE_CODES = new Dictionary<string, string>()
        {
             { "de", "ge" }, // German.
             { "en", "en" }, // English.
             { "es", "es" }, // Spanish.
             { "fr", "fr" }, // French.
             { "kr", "ko" }, // Korean.
             { "pt", "en" }, // Portuguese.
             { "ru", "ru" }, // Russian.
             { "th", "th" }, // Thai.
        };
        private string languageCode;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private JsonSerializerOptions _jsonSerializerOptions;
        public bool IsSupportingCurrentLanguage { get; private set; }

        public PoeNinjaClient(IHttpClientProvider httpClientProvider,
                              ILanguageProvider languageProvider,
                              ILogger logger)
        {
            httpClient = httpClientProvider.HttpClient;
            this.languageProvider = languageProvider;
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
            var url = new Uri($"{POE_NINJA_API_BASE_URL}itemoverview?league={leagueId}&type={itemType}&language={languageCode}");

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

            return new PoeNinjaQueryResult<PoeNinjaItem>() { Lines = new List<PoeNinjaItem>() };
        }

        public async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency)
        {
            var url = new Uri($"{POE_NINJA_API_BASE_URL}currencyoverview?league={leagueId}&type={currency}&language={languageCode}");

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

            return new PoeNinjaQueryResult<PoeNinjaCurrency>() { Lines = new List<PoeNinjaCurrency>() };
        }

        public Task OnAfterInit()
        {
            IsSupportingCurrentLanguage = POE_NINJA_LANGUAGE_CODES.TryGetValue(languageProvider.Current.LanguageCode, out languageCode);

            return Task.CompletedTask;
        }
    }
}
