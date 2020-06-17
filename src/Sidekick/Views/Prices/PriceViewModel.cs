using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Languages;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Extensions;
using Sidekick.Helpers;
using Sidekick.Localization.Prices;

namespace Sidekick.Views.Prices
{
    public class PriceViewModel : INotifyPropertyChanged
    {
        private readonly ILogger logger;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IPoeNinjaCache poeNinjaCache;
        private readonly IStaticDataService staticDataService;
        private readonly ILanguageProvider languageProvider;
        private readonly IPoePriceInfoClient poePriceInfoClient;
        private readonly INativeClipboard nativeClipboard;
        private readonly IParserService parserService;
        private readonly SidekickSettings settings;
        private readonly IStatDataService statDataService;

        public event PropertyChangedEventHandler PropertyChanged;

        public PriceViewModel(
            ILogger logger,
            ITradeSearchService tradeSearchService,
            IPoeNinjaCache poeNinjaCache,
            IStaticDataService staticDataService,
            ILanguageProvider languageProvider,
            IPoePriceInfoClient poePriceInfoClient,
            INativeClipboard nativeClipboard,
            IParserService parserService,
            SidekickSettings settings,
            IStatDataService statDataService)
        {
            this.logger = logger;
            this.tradeSearchService = tradeSearchService;
            this.poeNinjaCache = poeNinjaCache;
            this.staticDataService = staticDataService;
            this.languageProvider = languageProvider;
            this.poePriceInfoClient = poePriceInfoClient;
            this.nativeClipboard = nativeClipboard;
            this.parserService = parserService;
            this.settings = settings;
            this.statDataService = statDataService;
            Task.Run(Initialize);
        }

        public Item Item { get; private set; }

        public string ItemColor => Item?.GetColor();

        public ObservableList<PriceItem> Results { get; private set; }

        public Uri Uri => QueryResult?.Uri;

        public bool IsError { get; private set; }
        public bool IsFetching { get; private set; }

        public bool ShowFilters => !IsError;
        public bool ShowList => !IsError && !ShowRefresh;
        public bool ShowRefresh { get; private set; } = true;
        public bool ShowPreview => !IsError;

        public bool IsCurrency { get; private set; }

        public ObservableList<PriceFilterCategory> Filters { get; set; }

        private async Task Initialize()
        {
            Item = await parserService.ParseItem(nativeClipboard.LastCopiedText);

            if (Item == null)
            {
                IsError = true;
                return;
            }

            IsCurrency = Item.Rarity == Rarity.Currency;

            InitializeFilters();

            await UpdateQuery();

            GetPoeNinjaPrice();

            if (settings.EnablePricePrediction)
            {
                _ = GetPredictionPrice();
            }
        }

        private void InitializeFilters()
        {
            // No filters for prophecies, currencies and divination cards, etc.
            if (Item.Category == Category.DivinationCard
                || Item.Category == Category.Currency
                || Item.Category == Category.Prophecy
                || Item.Category == Category.ItemisedMonster
                || Item.Category == Category.Leaguestone
                || Item.Category == Category.Watchstone
                || Item.Category == Category.Undefined)
            {
                Filters = null;
                return;
            }

            Filters = new ObservableList<PriceFilterCategory>();

            var propertyCategory1 = new PriceFilterCategory();
            var propertyCategory2 = new PriceFilterCategory();

            // Quality
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.Quality), languageProvider.Language.DescriptionQuality, Item.Properties.Quality,
                enabled: Item.Rarity == Rarity.Gem,
                min: Item.Rarity == Rarity.Gem && Item.Properties.Quality >= 20 ? (double?)Item.Properties.Quality : null);

            // Armour
            InitializeFilter(propertyCategory1, nameof(SearchFilters.ArmourFilters), nameof(ArmorFilter.Armor), languageProvider.Language.DescriptionArmour, Item.Properties.Armor);
            // Evasion
            InitializeFilter(propertyCategory1, nameof(SearchFilters.ArmourFilters), nameof(ArmorFilter.Evasion), languageProvider.Language.DescriptionEvasion, Item.Properties.Evasion);
            // Energy shield
            InitializeFilter(propertyCategory1, nameof(SearchFilters.ArmourFilters), nameof(ArmorFilter.EnergyShield), languageProvider.Language.DescriptionEnergyShield, Item.Properties.EnergyShield);
            // Block
            InitializeFilter(propertyCategory1, nameof(SearchFilters.ArmourFilters), nameof(ArmorFilter.Block), languageProvider.Language.DescriptionChanceToBlock, Item.Properties.ChanceToBlock,
                delta: 1);

            // Gem level
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.GemLevel), languageProvider.Language.DescriptionLevel, Item.Properties.GemLevel,
                enabled: true,
                min: Item.Properties.GemLevel);

            // Item quantity
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MapFilters), nameof(MapFilter.ItemQuantity), languageProvider.Language.DescriptionItemQuantity, Item.Properties.ItemQuantity);
            // Item rarity
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MapFilters), nameof(MapFilter.ItemRarity), languageProvider.Language.DescriptionItemRarity, Item.Properties.ItemRarity);
            // Monster pack size
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MapFilters), nameof(MapFilter.MonsterPackSize), languageProvider.Language.DescriptionMonsterPackSize, Item.Properties.MonsterPackSize);
            // Blighted
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MapFilters), nameof(MapFilter.Blighted), languageProvider.Language.PrefixBlighted, Item.Properties.Blighted,
                enabled: Item.Properties.Blighted);
            // Map tier
            InitializeFilter(propertyCategory1, nameof(SearchFilters.MapFilters), nameof(MapFilter.MapTier), languageProvider.Language.DescriptionMapTier, Item.Properties.MapTier,
                enabled: true,
                min: Item.Properties.MapTier);

            // Physical Dps
            InitializeFilter(propertyCategory1, nameof(SearchFilters.WeaponFilters), nameof(WeaponFilter.PhysicalDps), PriceResources.Filters_PDps, Item.Properties.PhysicalDps);
            // Elemental Dps
            InitializeFilter(propertyCategory1, nameof(SearchFilters.WeaponFilters), nameof(WeaponFilter.ElementalDps), PriceResources.Filters_EDps, Item.Properties.ElementalDps);
            // Total Dps
            InitializeFilter(propertyCategory1, nameof(SearchFilters.WeaponFilters), nameof(WeaponFilter.DamagePerSecond), PriceResources.Filters_Dps, Item.Properties.DamagePerSecond);
            // Attacks per second
            InitializeFilter(propertyCategory1, nameof(SearchFilters.WeaponFilters), nameof(WeaponFilter.AttacksPerSecond), languageProvider.Language.DescriptionAttacksPerSecond, Item.Properties.AttacksPerSecond,
                delta: 0.1);
            // Critical strike chance
            InitializeFilter(propertyCategory1, nameof(SearchFilters.WeaponFilters), nameof(WeaponFilter.CriticalStrikeChance), languageProvider.Language.DescriptionCriticalStrikeChance, Item.Properties.CriticalStrikeChance,
                delta: 1);

            // Item level
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.ItemLevel), languageProvider.Language.DescriptionItemLevel, Item.ItemLevel,
                enabled: Item.ItemLevel >= 80 && Item.Properties.MapTier == 0 && Item.Rarity != Rarity.Unique,
                min: Item.ItemLevel >= 80 ? (double?)Item.ItemLevel : null);

            // Corrupted
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.Corrupted), languageProvider.Language.DescriptionCorrupted, Item.Corrupted,
                alwaysIncluded: Item.Rarity == Rarity.Gem || Item.Rarity == Rarity.Unique,
                enabled: (Item.Rarity == Rarity.Gem || Item.Rarity == Rarity.Unique) && Item.Corrupted,
                applyNegative: true);

            // Crusader
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.CrusaderItem), languageProvider.Language.InfluenceCrusader, Item.Influences.Crusader,
                enabled: Item.Influences.Crusader);
            // Elder
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.ElderItem), languageProvider.Language.InfluenceElder, Item.Influences.Elder,
                enabled: Item.Influences.Elder);
            // Hunter
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.HunterItem), languageProvider.Language.InfluenceHunter, Item.Influences.Hunter,
                enabled: Item.Influences.Hunter);
            // Redeemer
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.RedeemerItem), languageProvider.Language.InfluenceRedeemer, Item.Influences.Redeemer,
                enabled: Item.Influences.Redeemer);
            // Shaper
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.ShaperItem), languageProvider.Language.InfluenceShaper, Item.Influences.Shaper,
                enabled: Item.Influences.Shaper);
            // Warlord
            InitializeFilter(propertyCategory2, nameof(SearchFilters.MiscFilters), nameof(MiscFilter.WarlordItem), languageProvider.Language.InfluenceWarlord, Item.Influences.Warlord,
                enabled: Item.Influences.Warlord);

            if (propertyCategory1.Filters.Any())
            {
                Filters.Add(propertyCategory1);
            }

            if (propertyCategory2.Filters.Any())
            {
                Filters.Add(propertyCategory2);
            }

            // Modifiers
            InitializeMods(Item.Modifiers.Pseudo);
            InitializeMods(Item.Modifiers.Enchant, false);
            InitializeMods(Item.Modifiers.Implicit);
            InitializeMods(Item.Modifiers.Explicit);
            InitializeMods(Item.Modifiers.Crafted);

            if (Filters.Count == 0)
            {
                Filters = null;
            }
        }

        private void InitializeMods(List<Modifier> modifiers, bool normalizeValues = true)
        {
            if (modifiers.Count == 0)
            {
                return;
            }

            PriceFilterCategory category = null;

            var settingMods = Item.Category switch
            {
                Category.Accessory => settings.AccessoryModifiers,
                Category.Armour => settings.ArmourModifiers,
                Category.Flask => settings.FlaskModifiers,
                Category.Jewel => settings.JewelModifiers,
                Category.Map => settings.MapModifiers,
                Category.Weapon => settings.WeaponModifiers,
                _ => new List<string>(),
            };

            foreach (var modifier in modifiers)
            {
                if (category == null)
                {
                    category = new PriceFilterCategory();
                }

                if (modifier.OptionValue != null)
                {
                    InitializeFilter(category,
                                     nameof(StatFilter),
                                     modifier.Id,
                                     !string.IsNullOrEmpty(modifier.Category) ? $"({modifier.Category}) {modifier.Text}" : modifier.Text,
                                     modifier.OptionValue,
                                     normalizeValues: normalizeValues,
                                     enabled: settingMods.Contains(modifier.Id) && Item.Rarity != Rarity.Unique
                    );
                }
                else
                {
                    InitializeFilter(category,
                                     nameof(StatFilter),
                                     modifier.Id,
                                     !string.IsNullOrEmpty(modifier.Category) ? $"({modifier.Category}) {modifier.Text}" : modifier.Text,
                                     modifier.Values,
                                     normalizeValues: normalizeValues,
                                     enabled: settingMods.Contains(modifier.Id) && Item.Rarity != Rarity.Unique
                    );
                }
            }

            if (category != null)
            {
                Filters.Add(category);
            }
        }

        private static readonly Regex LabelValues = new Regex("(\\#)");
        private void InitializeFilter<T>(PriceFilterCategory category,
                                         string type,
                                         string id,
                                         string label,
                                         T value,
                                         double delta = 5,
                                         bool enabled = false,
                                         double? min = null,
                                         double? max = null,
                                         bool alwaysIncluded = false,
                                         bool normalizeValues = true,
                                         bool applyNegative = false)
        {
            ModifierOption option = null;

            if (value is bool boolValue)
            {
                if (!boolValue && !alwaysIncluded)
                {
                    return;
                }
            }
            else if (value is int intValue)
            {
                if (intValue == 0 && !alwaysIncluded)
                {
                    return;
                }
                if (min == null && normalizeValues)
                {
                    min = NormalizeMinValue(intValue, delta);
                }
                if (LabelValues.IsMatch(label))
                {
                    label = LabelValues.Replace(label, intValue.ToString());
                }
                else
                {
                    label += $": {value}";
                }
            }
            else if (value is double doubleValue)
            {
                if (doubleValue == 0 && !alwaysIncluded)
                {
                    return;
                }
                if (min == null && normalizeValues)
                {
                    min = NormalizeMinValue(doubleValue, delta);
                }
                if (LabelValues.IsMatch(label))
                {
                    label = LabelValues.Replace(label, doubleValue.ToString("0.00"));
                }
                else
                {
                    label += $": {doubleValue:0.00}";
                }
            }
            else if (value is List<double> groupValue)
            {
                var itemValue = groupValue.OrderBy(x => x).FirstOrDefault();

                if (itemValue >= 0)
                {
                    min = itemValue;
                    if (normalizeValues)
                    {
                        min = NormalizeMinValue(min, delta);
                    }
                }
                else
                {
                    max = itemValue;
                    if (normalizeValues)
                    {
                        max = NormalizeMaxValue(max, delta);
                    }
                }
            }
            else if (value is ModifierOption modifierOption)
            {
                option = modifierOption;
            }

            var priceFilter = new PriceFilter()
            {
                Enabled = enabled,
                Type = type,
                Id = id,
                Text = label,
                Min = min,
                Max = max,
                HasRange = min.HasValue || max.HasValue,
                ApplyNegative = applyNegative,
                Option = option,
            };

            priceFilter.PropertyChanged += async (object sender, PropertyChangedEventArgs e) => { await UpdateDebounce(); };

            category.Filters.Add(priceFilter);
        }

        private FetchResult<string> QueryResult { get; set; }

        /// <summary>
        /// Smallest positive value between a -5 delta or 90%.
        /// </summary>
        private int? NormalizeMinValue(double? value, double delta)
        {
            if (value.HasValue)
            {
                if (value.Value > 0)
                {
                    return (int)Math.Max(Math.Min(value.Value - delta, value.Value * 0.9), 0);
                }
                else
                {
                    return (int)Math.Min(Math.Min(value.Value - delta, value.Value * 1.1), 0);
                }
            }

            return null;
        }

        /// <summary>
        /// Smallest positive value between a -5 delta or 90%.
        /// </summary>
        private int? NormalizeMaxValue(double? value, double delta)
        {
            if (value.HasValue)
            {
                if (value.Value > 0)
                {
                    return (int)Math.Max(Math.Max(value.Value + delta, value.Value * 1.1), 0);
                }
                else
                {
                    return (int)Math.Min(Math.Max(value.Value + delta, value.Value * 0.9), 0);
                }
            }

            return null;
        }

        public int UpdateCountdown { get; private set; }
        private int UpdateCount = 0;
        public async Task UpdateDebounce()
        {
            var count = ++UpdateCount;
            UpdateCountdown = 2;
            ShowRefresh = true;
            while (UpdateCountdown > 0)
            {
                await Task.Delay(1000);
                if (count != UpdateCount)
                {
                    continue;
                }
                UpdateCountdown -= 1;
            }
            if (count == UpdateCount && ShowRefresh)
            {
                await UpdateQuery();
            }
        }

        public async Task UpdateQuery()
        {
            ShowRefresh = false;
            Results = new ObservableList<PriceItem>();

            if (Filters != null)
            {
                var saveSettings = false;

                var settingMods = Item.Category switch
                {
                    Category.Accessory => settings.AccessoryModifiers,
                    Category.Armour => settings.ArmourModifiers,
                    Category.Flask => settings.FlaskModifiers,
                    Category.Jewel => settings.JewelModifiers,
                    Category.Map => settings.MapModifiers,
                    Category.Weapon => settings.WeaponModifiers,
                    _ => new List<string>(),
                };

                foreach (var filter in Filters.SelectMany(x => x.Filters).Where(x => x.Type == nameof(StatFilter)))
                {
                    if (settingMods.Contains(filter.Id))
                    {
                        if (!filter.Enabled)
                        {
                            saveSettings = true;
                            settingMods.Remove(filter.Id);
                        }
                    }
                    else
                    {
                        if (filter.Enabled)
                        {
                            saveSettings = true;
                            settingMods.Add(filter.Id);
                        }
                    }
                }
                if (saveSettings)
                {
                    switch (Item.Category)
                    {
                        case Category.Accessory: settings.AccessoryModifiers = settingMods; break;
                        case Category.Armour: settings.ArmourModifiers = settingMods; break;
                        case Category.Flask: settings.FlaskModifiers = settingMods; break;
                        case Category.Jewel: settings.JewelModifiers = settingMods; break;
                        case Category.Map: settings.MapModifiers = settingMods; break;
                        case Category.Weapon: settings.WeaponModifiers = settingMods; break;
                    };

                    settings.Save();
                }
            }

            IsFetching = true;
            if (Item.Rarity == Rarity.Currency && staticDataService.GetId(Item) != null)
            {
                QueryResult = await tradeSearchService.SearchBulk(Item);
            }
            else
            {
                QueryResult = await tradeSearchService.Search(Item, GetFilters(), GetStats());
            }
            IsFetching = false;
            if (QueryResult == null)
            {
                IsError = true;
            }
            else if (QueryResult.Result.Any())
            {
                await LoadMoreData();
                await LoadMoreData();
            }

            UpdateCountString();
        }

        private List<StatFilter> GetStats()
        {
            return Filters?
                .SelectMany(x => x.Filters)
                .Where(x => x.Type == nameof(StatFilter))
                .Where(x => x.Enabled)
                .Select(x => new StatFilter()
                {
                    Disabled = !x.Enabled,
                    Id = x.Id,
                    Value = new SearchFilterValue()
                    {
                        Max = x.Max,
                        Min = x.Min,
                        Option = x.Option?.Value,
                    }
                })
                .ToList();
        }

        private SearchFilters GetFilters()
        {
            var filters = Filters?
                .SelectMany(x => x.Filters);

            if (filters == null)
            {
                return null;
            }

            var searchFilters = new SearchFilters();

            foreach (var filter in filters)
            {
                var property = searchFilters.GetType().GetProperty(filter.Type);
                if (property == null)
                {
                    continue;
                }

                var typeObject = property.GetValue(searchFilters);
                property = typeObject.GetType().GetProperty("Filters");
                if (property == null)
                {
                    continue;
                }

                var filtersObject = property.GetValue(typeObject);
                property = filtersObject.GetType().GetProperty(filter.Id);
                if (property == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(SearchFilterOption) && (filter.Enabled || filter.ApplyNegative))
                {
                    property.SetValue(filtersObject, new SearchFilterOption(filter.Enabled ? "true" : "false"));
                }
                else if (property.PropertyType == typeof(SearchFilterValue))
                {
                    if (!filter.Enabled)
                    {
                        continue;
                    }
                    var valueObject = new SearchFilterValue
                    {
                        Max = filter.Max,
                        Min = filter.Min,
                        Option = filter.Option?.Value,
                    };
                    property.SetValue(filtersObject, valueObject);
                }
            }

            return searchFilters;
        }

        public async Task LoadMoreData()
        {
            if (IsFetching)
            {
                return;
            }

            try
            {
                var ids = QueryResult.Result.Skip(Results?.Count ?? 0).Take(10).ToList();
                if (ids.Count == 0)
                {
                    return;
                }

                IsFetching = true;
                var getResult = await tradeSearchService.GetResults(QueryResult.Id, ids, GetStats());
                IsFetching = false;

                if (getResult != null && getResult.Result.Any())
                {
                    Results.AddRange(getResult.Result.Select(result => new PriceItem(result)
                    {
                        ImageUrl = new Uri(
                            languageProvider.Language.PoeCdnBaseUrl,
                            staticDataService.GetImage(result.Listing.Price.Currency)
                        ).AbsoluteUri,
                    }));
                }

                UpdateCountString();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error loading more data");
            }
        }

        public bool IsPredicted => !string.IsNullOrEmpty(PredictionText);
        public string PredictionText { get; private set; }
        private async Task GetPredictionPrice()
        {
            PredictionText = string.Empty;

            if (!IsPoeNinja)
            {
                var result = await poePriceInfoClient.GetItemPricePrediction(Item.Text);
                if (result?.ErrorCode != 0)
                {
                    return;
                }

                PredictionText = string.Format(
                    PriceResources.PredictionString,
                    $"{result.Min?.ToString("N1")}-{result.Max?.ToString("N1")} {result.Currency}",
                    result.ConfidenceScore.ToString("N1"));
            }
        }

        public bool IsPoeNinja => !string.IsNullOrEmpty(PoeNinjaText);
        public string PoeNinjaText { get; private set; }
        private void GetPoeNinjaPrice()
        {
            PoeNinjaText = string.Empty;

            var poeNinjaItemPriceInChaos = poeNinjaCache.GetItemPrice(Item);
            if (poeNinjaItemPriceInChaos != null)
            {
                PoeNinjaText = string.Format(PriceResources.PoeNinjaString,
                                             poeNinjaItemPriceInChaos.Value.ToString("N3"),
                                             poeNinjaCache.LastRefreshTimestamp.Value.ToString("t"));
            }
        }

        public bool FullyLoaded { get; private set; }
        public string CountString { get; private set; }
        private void UpdateCountString()
        {
            FullyLoaded = (Results?.Count ?? 0) == (QueryResult?.Result?.Count ?? 0);
            CountString = string.Format(PriceResources.CountString, Results?.Count ?? 0, QueryResult?.Total.ToString() ?? "?");
        }

        public bool HasPreviewItem { get; private set; }
        public PriceItem PreviewItem { get; private set; }
        public void Preview(PriceItem selectedItem)
        {
            PreviewItem = selectedItem;
            HasPreviewItem = PreviewItem != null;
        }
    }
}
