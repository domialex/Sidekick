using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Common.Game.Items;
using Sidekick.Common.Game.Items.Modifiers;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Domain.Game.Trade.Queries;

namespace Sidekick.Application.Game.Trade
{
    public class GetModifierFiltersHandler : IQueryHandler<GetModifierFilters, ModifierFilters>
    {
        public Task<ModifierFilters> Handle(GetModifierFilters request, CancellationToken cancellationToken)
        {
            var result = new ModifierFilters();

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

            List<string> enabledModifiers = new();
            // if (request.Item.Metadata.Rarity == Rarity.Rare || request.Item.Metadata.Rarity == Rarity.Magic)
            // {
            //     enabledModifiers = request.Item.Metadata.Category switch
            //     {
            //         Category.Accessory => settings.Price_Mods_Accessory,
            //         Category.Armour => settings.Price_Mods_Armour,
            //         Category.Flask => settings.Price_Mods_Flask,
            //         Category.Jewel => settings.Price_Mods_Jewel,
            //         Category.Map => settings.Price_Mods_Map,
            //         Category.Weapon => settings.Price_Mods_Weapon,
            //         _ => enabledModifiers,
            //     };
            // }

            InitializeModifierFilters(result.Pseudo, request.Item.Modifiers.Pseudo, enabledModifiers);
            InitializeModifierFilters(result.Enchant, request.Item.Modifiers.Enchant, enabledModifiers, false);
            InitializeModifierFilters(result.Implicit, request.Item.Modifiers.Implicit, enabledModifiers);
            InitializeModifierFilters(result.Explicit, request.Item.Modifiers.Explicit, enabledModifiers);
            InitializeModifierFilters(result.Crafted, request.Item.Modifiers.Crafted, enabledModifiers);
            InitializeModifierFilters(result.Fractured, request.Item.Modifiers.Fractured, enabledModifiers);

            return Task.FromResult(result);
        }

        private static void InitializeModifierFilters(List<ModifierFilter> filters, List<Modifier> modifiers, List<string> enabledModifiers, bool normalizeValues = true)
        {
            if (modifiers.Count == 0) return;

            foreach (var modifier in modifiers)
            {
                if (modifier.OptionValue != null)
                {
                    InitializeModifierFilter(filters,
                        modifier,
                        modifier.OptionValue,
                        normalizeValues: normalizeValues,
                        enabled: enabledModifiers.Contains(modifier.Id)
                    );
                }
                else
                {
                    InitializeModifierFilter(filters,
                        modifier,
                        modifier.Values,
                        normalizeValues: normalizeValues,
                        enabled: enabledModifiers.Contains(modifier.Id)
                    );
                }
            }
        }

        private static void InitializeModifierFilter<T>(List<ModifierFilter> filters,
            Modifier modifier,
            T value,
            double delta = 5,
            bool enabled = false,
            bool normalizeValues = true)
        {
            double? min = null;
            double? max = null;

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

            filters.Add(new ModifierFilter()
            {
                Enabled = enabled,
                Modifier = modifier,
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

        /// <summary>
        /// Smallest positive value between a +5 delta or 110%.
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
    }
}
