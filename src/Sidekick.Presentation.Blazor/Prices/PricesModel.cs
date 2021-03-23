using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Apis.PoePriceInfo.Queries;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Modifiers.Models;
using Sidekick.Domain.Game.Trade;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Domain.Game.Trade.Queries;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;
using Sidekick.Localization.Prices;
using Sidekick.Persistence.ItemCategories;
using Sidekick.Presentation.Blazor.Debounce;
using Sidekick.Presentation.Blazor.Extensions;


namespace Sidekick.Presentation.Blazor.Prices
{
    public class PricesModel : IDisposable
    {

        private readonly ILogger logger;
        private readonly IDebouncer debouncer;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IItemStaticDataProvider itemStaticDataProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ISidekickSettings settings;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IMediator mediator;
        private readonly PriceResources resources;

        public PricesModel(
            ILogger<PricesModel> logger,
            IDebouncer debouncer,
            ITradeSearchService tradeSearchService,
            IItemStaticDataProvider itemStaticDataProvider,
            IGameLanguageProvider gameLanguageProvider,
            ISidekickSettings settings,
            IItemCategoryRepository itemCategoryRepository,
            IMediator mediator,
            PriceResources resources)
        {
            this.logger = logger;
            this.debouncer = debouncer;
            this.tradeSearchService = tradeSearchService;
            this.itemStaticDataProvider = itemStaticDataProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.settings = settings;
            this.itemCategoryRepository = itemCategoryRepository;
            this.mediator = mediator;
            this.resources = resources;

            //
        }

        public Item Item { get; private set; }

        public string ItemColor => Item?.GetColor();

        public List<PriceItem> Results { get; private set; }

        public bool IsError { get; private set; }
        public bool IsFetching { get; private set; }

        public bool ShowFilters => !IsError;
        public bool ShowList => !IsError && !ShowRefresh;
        public bool ShowRefresh { get; private set; } = false;
        public bool ShowPreview => !IsError;

        public List<PriceFilterCategory> Filters { get; set; }

        public Dictionary<string, string> CategoryOptions { get; private set; } = new Dictionary<string, string>();
        public string SelectedCategory { get; set; }
        public bool ShowCategory { get; set; }

        public async Task Initialize(string encodedItem)
        {
            await this.Initialize(JsonSerializer.Deserialize<Item>(encodedItem.DecodeUrl().DecodeBase64()));
        }

        public async Task Initialize(Item item)
        {
            Item = item;

            CategoryOptions.Add(Item.Original.Type, null);
            if (Item.Metadata.Category == Category.Weapon)
            {
                CategoryOptions.Add(resources.Class_Weapon, "weapon");
                CategoryOptions.Add(resources.Class_WeaponOne, "weapon.one");
                CategoryOptions.Add(resources.Class_WeaponOneMelee, "weapon.onemelee");
                CategoryOptions.Add(resources.Class_WeaponTwoMelee, "weapon.twomelee");
                CategoryOptions.Add(resources.Class_WeaponBow, "weapon.bow");
                CategoryOptions.Add(resources.Class_WeaponClaw, "weapon.claw");
                CategoryOptions.Add(resources.Class_WeaponDagger, "weapon.dagger");
                CategoryOptions.Add(resources.Class_WeaponRuneDagger, "weapon.runedagger");
                CategoryOptions.Add(resources.Class_WeaponOneAxe, "weapon.oneaxe");
                CategoryOptions.Add(resources.Class_WeaponOneMace, "weapon.onemace");
                CategoryOptions.Add(resources.Class_WeaponOneSword, "weapon.onesword");
                CategoryOptions.Add(resources.Class_WeaponSceptre, "weapon.sceptre");
                CategoryOptions.Add(resources.Class_WeaponStaff, "weapon.staff");
                CategoryOptions.Add(resources.Class_WeaponWarstaff, "weapon.warstaff");
                CategoryOptions.Add(resources.Class_WeaponTwoAxe, "weapon.twoaxe");
                CategoryOptions.Add(resources.Class_WeaponTwoMace, "weapon.twomace");
                CategoryOptions.Add(resources.Class_WeaponTwoSword, "weapon.twosword");
                CategoryOptions.Add(resources.Class_WeaponWand, "weapon.wand");
                CategoryOptions.Add(resources.Class_WeaponRod, "weapon.rod");
            }

            if (Item.Metadata.Category == Category.Armour)
            {
                CategoryOptions.Add(resources.Class_Armour, "armour");
                CategoryOptions.Add(resources.Class_ArmourChest, "armour.chest");
                CategoryOptions.Add(resources.Class_ArmourBoots, "armour.boots");
                CategoryOptions.Add(resources.Class_ArmourGloves, "armour.gloves");
                CategoryOptions.Add(resources.Class_ArmourHelmet, "armour.helmet");
                CategoryOptions.Add(resources.Class_ArmourShield, "armour.shield");
                CategoryOptions.Add(resources.Class_ArmourQuiver, "armour.quiver");
            }

            if (Item.Metadata.Category == Category.Accessory)
            {
                CategoryOptions.Add(resources.Class_Accessory, "accessory");
                CategoryOptions.Add(resources.Class_AccessoryAmulet, "accessory.amulet");
                CategoryOptions.Add(resources.Class_AccessoryBelt, "accessory.belt");
                CategoryOptions.Add(resources.Class_AccessoryRing, "accessory.ring");
            }

            if (Item.Metadata.Category == Category.Gem)
            {
                CategoryOptions.Add(resources.Class_Gem, "gem");
                CategoryOptions.Add(resources.Class_GemActive, "gem.activegem");
                CategoryOptions.Add(resources.Class_GemSupport, "gem.supportgem");
                CategoryOptions.Add(resources.Class_GemAwakenedSupport, "gem.supportgemplus");
            }

            if (Item.Metadata.Category == Category.Jewel)
            {
                CategoryOptions.Add(resources.Class_Jewel, "jewel");
                CategoryOptions.Add(resources.Class_JewelBase, "jewel.base");
                CategoryOptions.Add(resources.Class_JewelAbyss, "jewel.abyss");
                CategoryOptions.Add(resources.Class_JewelCluster, "jewel.cluster");
            }

            if (Item.Metadata.Category == Category.Flask)
            {
                CategoryOptions.Add(resources.Class_Flask, "flask");
            }

            if (Item.Metadata.Category == Category.Map)
            {
                CategoryOptions.Add(resources.Class_Map, "map");
                CategoryOptions.Add(resources.Class_MapFragment, "map.fragment");
                CategoryOptions.Add(resources.Class_MapScarab, "map.scarab");
            }

            if (Item.Metadata.Category == Category.Watchstone)
            {
                CategoryOptions.Add(resources.Class_Watchstone, "watchstone");
            }

            if (Item.Metadata.Category == Category.Leaguestone)
            {
                CategoryOptions.Add(resources.Class_Leaguestone, "leaguestone");
            }

            if (Item.Metadata.Category == Category.Prophecy)
            {
                CategoryOptions.Add(resources.Class_Prophecy, "prophecy");
            }

            if (Item.Metadata.Category == Category.DivinationCard)
            {
                CategoryOptions.Add(resources.Class_Card, "card");
            }

            if (Item.Metadata.Category == Category.ItemisedMonster)
            {
                CategoryOptions.Add(resources.Class_MonsterBeast, "monster.beast");
                CategoryOptions.Add(resources.Class_MonsterSample, "monster.sample");
            }

            if (Item.Metadata.Category == Category.Currency)
            {
                CategoryOptions.Add(resources.Class_Currency, "currency");
                CategoryOptions.Add(resources.Class_CurrencyPiece, "currency.piece");
                CategoryOptions.Add(resources.Class_CurrencyResonator, "currency.resonator");
                CategoryOptions.Add(resources.Class_CurrencyFossil, "currency.fossil");
                CategoryOptions.Add(resources.Class_CurrencyIncubator, "currency.incubator");
            }

            SelectedCategory = (await itemCategoryRepository.Get(Item.Original.Type))?.Category;
            if (!CategoryOptions.Values.Any(x => x == SelectedCategory))
            {
                SelectedCategory = null;
            }

            ShowCategory = (Item?.Metadata.Rarity == Rarity.Rare || Item?.Metadata.Rarity == Rarity.Magic || Item?.Metadata.Rarity == Rarity.Normal) && CategoryOptions.Count > 2;

            InitializeFilters();

            await UpdateQuery();

            if (settings.Price_Prediction_Enable)
            {
                await GetPredictionPrice();
            }

        }

        private void InitializeFilters()
        {
            // No filters for prophecies, currencies and divination cards, etc.
            if (Item.Metadata.Category == Category.DivinationCard
                || Item.Metadata.Category == Category.Currency
                || Item.Metadata.Category == Category.Prophecy
                || Item.Metadata.Category == Category.ItemisedMonster
                || Item.Metadata.Category == Category.Leaguestone
                || Item.Metadata.Category == Category.Watchstone
                || Item.Metadata.Category == Category.Undefined)
            {
                Filters = null;
                return;
            }

            Filters = new List<PriceFilterCategory>();

            var propertyCategory1 = new PriceFilterCategory();
            var propertyCategory2 = new PriceFilterCategory();

            // Quality
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Misc_Quality, gameLanguageProvider.Language.DescriptionQuality, Item.Properties.Quality,
                enabled: Item.Metadata.Rarity == Rarity.Gem,
                min: Item.Metadata.Rarity == Rarity.Gem && Item.Properties.Quality >= 20 ? (double?)Item.Properties.Quality : null);

            // Armour
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Armour_Armour, gameLanguageProvider.Language.DescriptionArmour, Item.Properties.Armor);
            // Evasion
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Armour_Evasion, gameLanguageProvider.Language.DescriptionEvasion, Item.Properties.Evasion);
            // Energy shield
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Armour_EnergyShield, gameLanguageProvider.Language.DescriptionEnergyShield, Item.Properties.EnergyShield);
            // Block
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Armour_Block, gameLanguageProvider.Language.DescriptionChanceToBlock, Item.Properties.ChanceToBlock,
                delta: 1);

            // Gem level
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Misc_GemLevel, gameLanguageProvider.Language.DescriptionLevel, Item.Properties.GemLevel,
                enabled: true,
                min: Item.Properties.GemLevel);

            // Item quantity
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Map_ItemQuantity, gameLanguageProvider.Language.DescriptionItemQuantity, Item.Properties.ItemQuantity);
            // Item rarity
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Map_ItemRarity, gameLanguageProvider.Language.DescriptionItemRarity, Item.Properties.ItemRarity);
            // Monster pack size
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Map_MonsterPackSize, gameLanguageProvider.Language.DescriptionMonsterPackSize, Item.Properties.MonsterPackSize);
            // Blighted
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Map_Blighted, gameLanguageProvider.Language.PrefixBlighted, Item.Properties.Blighted,
                enabled: Item.Properties.Blighted);
            // Map tier
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Map_Tier, gameLanguageProvider.Language.DescriptionMapTier, Item.Properties.MapTier,
                enabled: true,
                min: Item.Properties.MapTier);

            // Physical Dps
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Weapon_PhysicalDps, resources.Filters_PDps, Item.Properties.PhysicalDps);
            // Elemental Dps
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Weapon_ElementalDps, resources.Filters_EDps, Item.Properties.ElementalDps);
            // Total Dps
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Weapon_Dps, resources.Filters_Dps, Item.Properties.DamagePerSecond);
            // Attacks per second
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Weapon_AttacksPerSecond, gameLanguageProvider.Language.DescriptionAttacksPerSecond, Item.Properties.AttacksPerSecond,
                delta: 0.1);
            // Critical strike chance
            InitializePropertyFilter(propertyCategory1, PropertyFilterType.Weapon_CriticalStrikeChance, gameLanguageProvider.Language.DescriptionCriticalStrikeChance, Item.Properties.CriticalStrikeChance,
                delta: 1);

            // Item level
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Misc_ItemLevel, gameLanguageProvider.Language.DescriptionItemLevel, Item.Properties.ItemLevel,
                enabled: Item.Properties.ItemLevel >= 80 && Item.Properties.MapTier == 0 && Item.Metadata.Rarity != Rarity.Unique,
                min: Item.Properties.ItemLevel >= 80 ? (double?)Item.Properties.ItemLevel : null);

            // Corrupted
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Misc_Corrupted, gameLanguageProvider.Language.DescriptionCorrupted, Item.Properties.Corrupted,
                alwaysIncluded: Item.Metadata.Rarity == Rarity.Gem || Item.Metadata.Rarity == Rarity.Unique,
                enabled: (Item.Metadata.Rarity == Rarity.Gem || Item.Metadata.Rarity == Rarity.Unique || Item.Metadata.Rarity == Rarity.Rare) && Item.Properties.Corrupted);

            // Crusader
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Crusader, gameLanguageProvider.Language.InfluenceCrusader, Item.Influences.Crusader,
                enabled: Item.Influences.Crusader);
            // Elder
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Elder, gameLanguageProvider.Language.InfluenceElder, Item.Influences.Elder,
                enabled: Item.Influences.Elder);
            // Hunter
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Hunter, gameLanguageProvider.Language.InfluenceHunter, Item.Influences.Hunter,
                enabled: Item.Influences.Hunter);
            // Redeemer
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Redeemer, gameLanguageProvider.Language.InfluenceRedeemer, Item.Influences.Redeemer,
                enabled: Item.Influences.Redeemer);
            // Shaper
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Shaper, gameLanguageProvider.Language.InfluenceShaper, Item.Influences.Shaper,
                enabled: Item.Influences.Shaper);
            // Warlord
            InitializePropertyFilter(propertyCategory2, PropertyFilterType.Influence_Warlord, gameLanguageProvider.Language.InfluenceWarlord, Item.Influences.Warlord,
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
            InitializeModifierFilters(Item.Modifiers.Pseudo);
            InitializeModifierFilters(Item.Modifiers.Enchant, false);
            InitializeModifierFilters(Item.Modifiers.Implicit);
            InitializeModifierFilters(Item.Modifiers.Explicit);
            InitializeModifierFilters(Item.Modifiers.Crafted);
            InitializeModifierFilters(Item.Modifiers.Fractured);

            if (Filters.Count == 0)
            {
                Filters = null;
            }
        }

        private void InitializeModifierFilters(List<Modifier> modifiers, bool normalizeValues = true)
        {
            if (modifiers.Count == 0)
            {
                return;
            }

            PriceFilterCategory category = null;

            var settingMods = Item.Metadata.Category switch
            {
                Category.Accessory => settings.Price_Mods_Accessory,
                Category.Armour => settings.Price_Mods_Armour,
                Category.Flask => settings.Price_Mods_Flask,
                Category.Jewel => settings.Price_Mods_Jewel,
                Category.Map => settings.Price_Mods_Map,
                Category.Weapon => settings.Price_Mods_Weapon,
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
                    InitializeModifierFilter(category,
                                     modifier.Id,
                                     !string.IsNullOrEmpty(modifier.Category) ? $"({modifier.Category}) {modifier.Text}" : modifier.Text,
                                     modifier.OptionValue,
                                     normalizeValues: normalizeValues,
                                     enabled: settingMods.Contains(modifier.Id) && Item.Metadata.Rarity != Rarity.Unique
                    );
                }
                else
                {
                    InitializeModifierFilter(category,
                                     modifier.Id,
                                     !string.IsNullOrEmpty(modifier.Category) ? $"({modifier.Category}) {modifier.Text}" : modifier.Text,
                                     modifier.Values,
                                     normalizeValues: normalizeValues,
                                     enabled: settingMods.Contains(modifier.Id) && Item.Metadata.Rarity != Rarity.Unique
                    );
                }
            }

            if (category != null)
            {
                Filters.Add(category);
            }
        }

        private static readonly Regex LabelValues = new Regex("(\\#)");

        private void InitializeModifierFilter<T>(PriceFilterCategory category,
                                         string id,
                                         string label,
                                         T value,
                                         double delta = 5,
                                         bool enabled = false,
                                         double? min = null,
                                         double? max = null,
                                         bool normalizeValues = true)
        {
            ModifierOption option = null;

            if (value is List<double> groupValue)
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

                if (!groupValue.Any())
                {
                    min = null;
                    max = null;
                }
            }

            var priceFilter = new PriceFilter()
            {
                Enabled = enabled,
                PropertyType = null,
                Id = id,
                Text = label,
                Min = min,
                Max = max,
                HasRange = min.HasValue || max.HasValue,
                Option = option,
            };

            category.Filters.Add(priceFilter);
        }

        private void InitializePropertyFilter<T>(PriceFilterCategory category,
                                         PropertyFilterType type,
                                         string label,
                                         T value,
                                         double delta = 5,
                                         bool enabled = false,
                                         double? min = null,
                                         double? max = null,
                                         bool alwaysIncluded = false,
                                         bool normalizeValues = true)
        {
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

            var priceFilter = new PriceFilter()
            {
                Enabled = enabled,
                PropertyType = type,
                Text = label,
                Min = min,
                Max = max,
                HasRange = min.HasValue || max.HasValue,
                Option = null,
            };

            priceFilter.PropertyChanged += (object sender, PropertyChangedEventArgs e) => { UpdateDebounce(); };

            category.Filters.Add(priceFilter);
        }

        private TradeSearchResult<string> QueryResult { get; set; }

        /// <summary>
        /// Smallest positive value between a -5 delta or 90%.
        /// </summary>
        private static int? NormalizeMinValue(double? value, double delta)
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
        private static int? NormalizeMaxValue(double? value, double delta)
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

        public Uri Uri { get; set; }

        public int UpdateCountdown { get; private set; }

        public void UpdateDebounce()
        {
            ShowRefresh = true;

            _ = debouncer.Debounce("priceview", async () => await UpdateQuery(),
                delayUpdate: (countdown) =>
                {
                    UpdateCountdown = (int)Math.Round((decimal)countdown / 1000);
                });
        }

        public async Task UpdateQuery()
        {
            ShowRefresh = false;
            Results = new List<PriceItem>();

            if (Filters != null)
            {
                var settingMods = Item.Metadata.Category switch
                {
                    Category.Accessory => settings.Price_Mods_Accessory,
                    Category.Armour => settings.Price_Mods_Armour,
                    Category.Flask => settings.Price_Mods_Flask,
                    Category.Jewel => settings.Price_Mods_Jewel,
                    Category.Map => settings.Price_Mods_Map,
                    Category.Weapon => settings.Price_Mods_Weapon,
                    _ => new List<string>(),
                };

                foreach (var filter in Filters.SelectMany(x => x.Filters))
                {
                    if (settingMods.Contains(filter.Id))
                    {
                        if (!filter.Enabled)
                        {
                            settingMods.Remove(filter.Id);
                        }
                    }
                    else
                    {
                        if (filter.Enabled)
                        {
                            settingMods.Add(filter.Id);
                        }
                    }
                }

                await mediator.Send(new SaveSettingsCommand(settings));
            }

            IsFetching = true;
            if (Item.Metadata.Rarity == Rarity.Currency && itemStaticDataProvider.GetId(Item) != null)
            {
                QueryResult = await tradeSearchService.SearchBulk(Item);
            }
            else
            {
                QueryResult = await tradeSearchService.Search(Item, GetPropertyFilters(), GetModifierFilters());
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

            if (!string.IsNullOrEmpty(QueryResult?.Id))
            {
                Uri = await mediator.Send(new GetTradeUriQuery(Item, QueryResult.Id));
            }
            else
            {
                Uri = null;
            }

            UpdateCountString();
        }

        private List<PropertyFilter> GetPropertyFilters()
        {
            return Filters?
                .SelectMany(x => x.Filters)
                .Where(x => x.PropertyType.HasValue)
                .Select(x => new PropertyFilter()
                {
                    Enabled = x.Enabled,
                    Max = x.Max,
                    Min = x.Min,
                    Type = x.PropertyType.Value,
                })
                .ToList();
        }

        private List<ModifierFilter> GetModifierFilters()
        {
            return Filters?
                .SelectMany(x => x.Filters)
                .Where(x => !x.PropertyType.HasValue)
                .Where(x => x.Enabled)
                .Select(x => new ModifierFilter()
                {
                    Id = x.Id,
                    Max = x.Max,
                    Min = x.Min,
                    Value = x.Option?.Value,
                })
                .ToList();
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
                var result = await tradeSearchService.GetResults(QueryResult.Id, ids, GetModifierFilters());
                IsFetching = false;

                if (result != null && result.Any())
                {
                    Results.AddRange(result.Select(x => new PriceItem(x, resources)
                    {
                        ImageUrl = new Uri($"{gameLanguageProvider.Language.PoeCdnBaseUrl}{itemStaticDataProvider.GetImage(x.Price.Currency)}").AbsoluteUri,
                    }));
                }

                UpdateCountString();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error loading more data");
            }
        }

        public string PoeNinjaText { get; private set; }

        public string PredictionText { get; private set; }

        private async Task GetPredictionPrice()
        {
            PredictionText = string.Empty;

            if (string.IsNullOrEmpty(PoeNinjaText))
            {
                var result = await mediator.Send(new GetPricePredictionQuery(Item));
                if (result == null || (result.Min == 0 && result.Max == 0))
                {
                    return;
                }

                PredictionText = string.Format(
                    resources.PredictionString,
                    $"{result.Min:N1}-{result.Max:N1} {result.Currency}",
                    result.ConfidenceScore.ToString("N1"));
            }
        }

        public bool FullyLoaded { get; private set; }
        public string CountString { get; private set; }

        private void UpdateCountString()
        {
            FullyLoaded = (Results?.Count ?? 0) == (QueryResult?.Result?.Count ?? 0);
            CountString = string.Format(resources.CountString, Results?.Count ?? 0, QueryResult?.Total.ToString() ?? "?");
        }

        public bool HasPreviewItem { get; private set; }
        public PriceItem PreviewItem { get; private set; }

        public void Preview(PriceItem selectedItem)
        {
            PreviewItem = selectedItem;
            HasPreviewItem = PreviewItem != null;
        }

        private void PriceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCategory))
            {
                if (string.IsNullOrEmpty(SelectedCategory))
                {
                    _ = itemCategoryRepository.Delete(Item.Original.Type);
                }
                else
                {
                    _ = itemCategoryRepository.SaveCategory(Item.Original.Type, SelectedCategory);
                }
                UpdateDebounce();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
