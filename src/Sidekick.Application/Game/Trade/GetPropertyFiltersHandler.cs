using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Domain.Game.Trade.Queries;
using Sidekick.Localization.Trade;

namespace Sidekick.Application.Game.Trade
{
    public class GetPropertyFiltersHandler : IQueryHandler<GetPropertyFilters, PropertyFilters>
    {
        private static readonly Regex LabelValues = new("(\\#)");

        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly TradeResources resources;

        public GetPropertyFiltersHandler(IGameLanguageProvider gameLanguageProvider,
            TradeResources resources)
        {
            this.gameLanguageProvider = gameLanguageProvider;
            this.resources = resources;
        }

        public Task<PropertyFilters> Handle(GetPropertyFilters request, CancellationToken cancellationToken)
        {
            var result = new PropertyFilters();

            // No filters for prophecies, currencies and divination cards, etc.
            if (request.Item.Metadata.Category == Category.DivinationCard
                || request.Item.Metadata.Category == Category.Currency
                || request.Item.Metadata.Category == Category.Prophecy
                || request.Item.Metadata.Category == Category.ItemisedMonster
                || request.Item.Metadata.Category == Category.Leaguestone
                || request.Item.Metadata.Category == Category.Watchstone
                || request.Item.Metadata.Category == Category.Undefined)
            {
                return Task.FromResult(result);
            }

            // Armour
            InitializePropertyFilter(result.Armour,
                PropertyFilterType.Armour_Armour,
                gameLanguageProvider.Language.DescriptionArmour,
                request.Item.Properties.Armor);
            // Evasion
            InitializePropertyFilter(result.Armour,
                PropertyFilterType.Armour_Evasion,
                gameLanguageProvider.Language.DescriptionEvasion,
                request.Item.Properties.Evasion);
            // Energy shield
            InitializePropertyFilter(result.Armour,
                PropertyFilterType.Armour_EnergyShield,
                gameLanguageProvider.Language.DescriptionEnergyShield,
                request.Item.Properties.EnergyShield);
            // Block
            InitializePropertyFilter(result.Armour,
                PropertyFilterType.Armour_Block,
                gameLanguageProvider.Language.DescriptionChanceToBlock,
                request.Item.Properties.ChanceToBlock,
                delta: 1);

            // Physical Dps
            InitializePropertyFilter(result.Weapon,
                PropertyFilterType.Weapon_PhysicalDps,
                resources.Filters_PDps,
                request.Item.Properties.PhysicalDps);
            // Elemental Dps
            InitializePropertyFilter(result.Weapon,
                PropertyFilterType.Weapon_ElementalDps,
                resources.Filters_EDps,
                request.Item.Properties.ElementalDps);
            // Total Dps
            InitializePropertyFilter(result.Weapon,
                PropertyFilterType.Weapon_Dps,
                resources.Filters_Dps,
                request.Item.Properties.DamagePerSecond);
            // Attacks per second
            InitializePropertyFilter(result.Weapon,
                PropertyFilterType.Weapon_AttacksPerSecond,
                gameLanguageProvider.Language.DescriptionAttacksPerSecond,
                request.Item.Properties.AttacksPerSecond,
                delta: 0.1);
            // Critical strike chance
            InitializePropertyFilter(result.Weapon,
                PropertyFilterType.Weapon_CriticalStrikeChance,
                gameLanguageProvider.Language.DescriptionCriticalStrikeChance,
                request.Item.Properties.CriticalStrikeChance,
                delta: 1);

            // Item quantity
            InitializePropertyFilter(result.Map,
                PropertyFilterType.Map_ItemQuantity,
                gameLanguageProvider.Language.DescriptionItemQuantity,
                request.Item.Properties.ItemQuantity);
            // Item rarity
            InitializePropertyFilter(result.Map,
                PropertyFilterType.Map_ItemRarity,
                gameLanguageProvider.Language.DescriptionItemRarity,
                request.Item.Properties.ItemRarity);
            // Monster pack size
            InitializePropertyFilter(result.Map,
                PropertyFilterType.Map_MonsterPackSize,
                gameLanguageProvider.Language.DescriptionMonsterPackSize,
                request.Item.Properties.MonsterPackSize);
            // Blighted
            InitializePropertyFilter(result.Map,
                PropertyFilterType.Map_Blighted,
                gameLanguageProvider.Language.PrefixBlighted,
                request.Item.Properties.Blighted,
                enabled: request.Item.Properties.Blighted);
            // Map tier
            InitializePropertyFilter(result.Map,
                PropertyFilterType.Map_Tier,
                gameLanguageProvider.Language.DescriptionMapTier,
                request.Item.Properties.MapTier,
                enabled: true,
                min: request.Item.Properties.MapTier);

            // Quality
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Quality,
                gameLanguageProvider.Language.DescriptionQuality,
                request.Item.Properties.Quality,
                enabled: request.Item.Metadata.Rarity == Rarity.Gem,
                min: request.Item.Metadata.Rarity == Rarity.Gem && request.Item.Properties.Quality >= 20 ? request.Item.Properties.Quality : null);
            // Gem level
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_GemLevel,
                gameLanguageProvider.Language.DescriptionLevel,
                request.Item.Properties.GemLevel,
                enabled: true,
                min: request.Item.Properties.GemLevel);
            // Item level
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_ItemLevel,
                gameLanguageProvider.Language.DescriptionItemLevel,
                request.Item.Properties.ItemLevel,
                enabled: request.Item.Properties.ItemLevel >= 80 && request.Item.Properties.MapTier == 0 && request.Item.Metadata.Rarity != Rarity.Unique,
                min: request.Item.Properties.ItemLevel >= 80 ? (double?)request.Item.Properties.ItemLevel : null);
            // Corrupted
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Corrupted,
                gameLanguageProvider.Language.DescriptionCorrupted,
                request.Item.Properties.Corrupted,
                enabled: (request.Item.Metadata.Rarity == Rarity.Gem || request.Item.Metadata.Rarity == Rarity.Unique || request.Item.Metadata.Rarity == Rarity.Rare) && request.Item.Properties.Corrupted);
            // Crusader
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Crusader,
                gameLanguageProvider.Language.InfluenceCrusader,
                request.Item.Influences.Crusader,
                enabled: request.Item.Influences.Crusader);
            // Elder
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Elder,
                gameLanguageProvider.Language.InfluenceElder,
                request.Item.Influences.Elder,
                enabled: request.Item.Influences.Elder);
            // Hunter
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Hunter,
                gameLanguageProvider.Language.InfluenceHunter,
                request.Item.Influences.Hunter,
                enabled: request.Item.Influences.Hunter);
            // Redeemer
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Redeemer,
                gameLanguageProvider.Language.InfluenceRedeemer,
                request.Item.Influences.Redeemer,
                enabled: request.Item.Influences.Redeemer);
            // Shaper
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Shaper,
                gameLanguageProvider.Language.InfluenceShaper,
                request.Item.Influences.Shaper,
                enabled: request.Item.Influences.Shaper);
            // Warlord
            InitializePropertyFilter(result.Misc,
                PropertyFilterType.Misc_Influence_Warlord,
                gameLanguageProvider.Language.InfluenceWarlord, request.Item.Influences.Warlord,
                enabled: request.Item.Influences.Warlord);

            return Task.FromResult(result);
        }

        private static void InitializePropertyFilter<T>(List<PropertyFilter> filters,
            PropertyFilterType type,
            string label,
            T value,
            double delta = 5,
            bool enabled = false,
            double? min = null,
            double? max = null)
        {
            FilterValueType valueType;

            switch (value)
            {
                case bool boolValue:
                    valueType = FilterValueType.Boolean;
                    if (!boolValue) return;
                    break;
                case int intValue:
                    valueType = FilterValueType.Int;
                    if (intValue == 0) return;
                    min ??= NormalizeMinValue(intValue, delta);
                    if (LabelValues.IsMatch(label))
                    {
                        label = LabelValues.Replace(label, intValue.ToString());
                    }
                    else
                    {
                        label += $": {value}";
                    }
                    break;
                case double doubleValue:
                    valueType = FilterValueType.Double;
                    if (doubleValue == 0) return;
                    min ??= NormalizeMinValue(doubleValue, delta);
                    if (LabelValues.IsMatch(label))
                    {
                        label = LabelValues.Replace(label, doubleValue.ToString("0.00"));
                    }
                    else
                    {
                        label += $": {doubleValue:0.00}";
                    }
                    break;
                default: return;
            }

            filters.Add(new PropertyFilter()
            {
                Enabled = enabled,
                Type = type,
                Value = value,
                ValueType = valueType,
                Text = label,
                Min = min,
                Max = max,
            });
        }

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
    }
}
