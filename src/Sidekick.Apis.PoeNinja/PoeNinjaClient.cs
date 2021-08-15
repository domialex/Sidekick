using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Apis.PoeNinja.Models;
using Sidekick.Apis.PoeNinja.Repository;
using Sidekick.Apis.PoeNinja.Repository.Models;
using Sidekick.Common.Game.Items;
using Sidekick.Common.Game.Languages;
using Sidekick.Common.Settings;

namespace Sidekick.Apis.PoeNinja
{
    /// <summary>
    /// https://poe.ninja/swagger
    /// </summary>
    public class PoeNinjaClient : IPoeNinjaClient
    {
        private readonly static Uri POE_NINJA_API_BASE_URL = new("https://poe.ninja/api/data/");
        /// <summary>
        /// Poe.ninja uses its own language codes.
        /// </summary>
        public readonly static Dictionary<string, string> POE_NINJA_LANGUAGE_CODES = new()
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
        private readonly IPoeNinjaRepository repository;
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
            ISettings settings,
            IPoeNinjaRepository repository)
        {
            this.logger = logger;
            this.gameLanguageProvider = gameLanguageProvider;
            this.settings = settings;
            this.repository = repository;
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

        public async Task<NinjaPrice> GetPriceInfo(Item item)
        {
            var cacheResult = await repository.Find(item);

            if (cacheResult != null && cacheResult.LastUpdated.AddHours(4) > DateTimeOffset.Now)
            {
                return cacheResult;
            }

            var fetchItems = new List<Task<PoeNinjaQueryResult<PoeNinjaItem>>>();
            var fetchCurrencies = new List<Task<PoeNinjaQueryResult<PoeNinjaCurrency>>>();

            if (item.Metadata.Category == Category.Currency)
            {
                fetchCurrencies.Add(FetchCurrencies(CurrencyType.Currency));
                fetchCurrencies.Add(FetchCurrencies(CurrencyType.Fragment));

                fetchItems.Add(FetchItems(ItemType.Incubator));
                fetchItems.Add(FetchItems(ItemType.Oil));
                fetchItems.Add(FetchItems(ItemType.Incubator));
                fetchItems.Add(FetchItems(ItemType.Scarab));
                fetchItems.Add(FetchItems(ItemType.Fossil));
                fetchItems.Add(FetchItems(ItemType.Resonator));
                fetchItems.Add(FetchItems(ItemType.Essence));
                fetchItems.Add(FetchItems(ItemType.Resonator));
            }
            else if (item.Metadata.Rarity == Rarity.Unique)
            {
                switch (item.Metadata.Category)
                {
                    case Category.Accessory: fetchItems.Add(FetchItems(ItemType.UniqueAccessory)); break;
                    case Category.Armour: fetchItems.Add(FetchItems(ItemType.UniqueArmour)); break;
                    case Category.Flask: fetchItems.Add(FetchItems(ItemType.UniqueFlask)); break;
                    case Category.Jewel: fetchItems.Add(FetchItems(ItemType.UniqueJewel)); break;
                    case Category.Map: fetchItems.Add(FetchItems(ItemType.UniqueMap)); break;
                    case Category.Weapon: fetchItems.Add(FetchItems(ItemType.UniqueWeapon)); break;
                    case Category.ItemisedMonster: fetchItems.Add(FetchItems(ItemType.Beast)); break;
                }
            }
            else
            {
                switch (item.Metadata.Category)
                {
                    case Category.DivinationCard: fetchItems.Add(FetchItems(ItemType.DivinationCard)); break;
                    case Category.Map: fetchItems.Add(FetchItems(ItemType.Map)); break;
                    case Category.Gem: fetchItems.Add(FetchItems(ItemType.SkillGem)); break;
                    case Category.Prophecy: fetchItems.Add(FetchItems(ItemType.Prophecy)); break;
                    case Category.ItemisedMonster: fetchItems.Add(FetchItems(ItemType.Beast)); break;
                }
            }

            if (fetchCurrencies.Count == 0 && fetchItems.Count == 0)
            {
                return null;
            }

            var itemResults = await Task.WhenAll(fetchItems);
            var items = itemResults
                .SelectMany(x => x.Lines)
                .Select(x => new NinjaPrice()
                {
                    Corrupted = x.Corrupted,
                    Price = x.ChaosValue,
                    LastUpdated = DateTimeOffset.Now,
                    Name = x.Name,
                    MapTier = x.MapTier,
                    GemLevel = x.GemLevel,
                })
                .ToList();
            await repository.SavePrices(items);
            await SaveTranslations(itemResults);

            var currencyResults = await Task.WhenAll(fetchCurrencies);
            var currencies = currencyResults
                .SelectMany(x => x.Lines)
                .Where(x => x.Receive != null)
                .Select(x => new NinjaPrice()
                {
                    Corrupted = false,
                    Price = x.Receive.Value,
                    LastUpdated = DateTimeOffset.Now,
                    Name = x.CurrencyTypeName,
                })
                .ToList();
            await repository.SavePrices(currencies);
            await SaveTranslations(currencyResults);

            return await repository.Find(item);
        }

        private async Task<PoeNinjaQueryResult<PoeNinjaItem>> FetchItems(ItemType itemType)
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

        private async Task<PoeNinjaQueryResult<PoeNinjaCurrency>> FetchCurrencies(CurrencyType currency)
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

        private async Task SaveTranslations<T>(PoeNinjaQueryResult<T>[] results)
        {
            var translations = results
                .SelectMany(x => x.Language.Translations)
                .Where(y => !y.Value.Contains("."))
                .Distinct()
                .Select(x => new NinjaTranslation()
                {
                    English = x.Key,
                    Translation = x.Value,
                })
                .ToList();
            await repository.SaveTranslations(translations);
        }
    }
}
