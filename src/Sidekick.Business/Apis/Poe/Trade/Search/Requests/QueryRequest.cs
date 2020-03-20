using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Requests
{
    public class QueryRequest
    {
        public QueryRequest(ParsedItem item, List<StatFilter> stats)
        {
            Query.Status.Option = StatusType.Online;
            Query.Filters.TradeFilters.Filters.SaleType = new SearchFilterOption { Option = "priced" };

            // Auto Search 5+ Links
            var highestCount = item.Sockets
                .GroupBy(x => x.Group)
                .Select(x => x.Count())
                .OrderByDescending(x => x)
                .FirstOrDefault();
            if (highestCount >= 5)
            {
                Query.Filters.SocketFilters.Filters.Links = new SocketFilterOption()
                {
                    Min = highestCount,
                };
            }

            if (item.Rarity == Rarity.Unique)
            {
                Query.Name = item.Name;
                Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                {
                    Option = "Unique",
                };
            }
            else if (item.Rarity == Rarity.Prophecy)
            {
                Query.Name = item.Name;
            }
            else
            {
                Query.Type = item.TypeLine;
                Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                {
                    Option = "nonunique",
                };

                if (item.ItemLevel >= 86)
                {
                    Query.Filters.MiscFilters.Filters.ItemLevel = new SearchFilterValue()
                    {
                        Min = 86
                    };
                }

                if (item.Influences.Crusader)
                {
                    Query.Filters.MiscFilters.Filters.CrusaderItem = new SearchFilterOption("true");
                }

                if (item.Influences.Elder)
                {
                    Query.Filters.MiscFilters.Filters.ElderItem = new SearchFilterOption("true");
                }

                if (item.Influences.Hunter)
                {
                    Query.Filters.MiscFilters.Filters.HunterItem = new SearchFilterOption("true");
                }

                if (item.Influences.Redeemer)
                {
                    Query.Filters.MiscFilters.Filters.RedeemerItem = new SearchFilterOption("true");
                }

                if (item.Influences.Shaper)
                {
                    Query.Filters.MiscFilters.Filters.ShaperItem = new SearchFilterOption("true");
                }

                if (item.Influences.Warlord)
                {
                    Query.Filters.MiscFilters.Filters.WarlordItem = new SearchFilterOption("true");
                }

                if (item.Armor > 0)
                {
                    Query.Filters.ArmourFilters.Filters.Armor = new SearchFilterValue() { Min = item.Armor - 20 };
                }
                if (item.EnergyShield > 0)
                {
                    Query.Filters.ArmourFilters.Filters.EnergyShield = new SearchFilterValue() { Min = item.EnergyShield - 20 };
                }
                if (item.Evasion > 0)
                {
                    Query.Filters.ArmourFilters.Filters.Evasion = new SearchFilterValue() { Min = item.Evasion - 20 };
                }

                if (!string.IsNullOrEmpty(item.PhysicalDamage) || !string.IsNullOrEmpty(item.ElementalDamage))
                {
                    var (physMin, physMax) = ParseRange(item.PhysicalDamage);
                    var (elMin, elMax) = ParseRange(item.ElementalDamage);

                    Query.Filters.WeaponFilters.Filters.APS = new SearchFilterValue() { Min = item.AttacksPerSecond };
                    Query.Filters.WeaponFilters.Filters.Crit = new SearchFilterValue() { Min = item.CriticalStrikeChance };
                    Query.Filters.WeaponFilters.Filters.EDPS = new SearchFilterValue() { Min = CalculateDps(elMin, elMax, item.AttacksPerSecond) };
                    Query.Filters.WeaponFilters.Filters.PDPS = new SearchFilterValue() { Min = CalculateDps(physMin, physMax, item.AttacksPerSecond) };
                }

                if (item.Level > 0)
                {
                    Query.Filters.MiscFilters.Filters.GemLevel = new SearchFilterValue()
                    {
                        Min = item.Level,
                    };
                }

                if (item.Rarity == Rarity.Gem)
                {
                    Query.Filters.MiscFilters.Filters.Quality = new SearchFilterValue()
                    {
                        Min = item.Quality,
                    };

                    if (item.Corrupted)
                    {
                        Query.Filters.MiscFilters.Filters.Corrupted = new SearchFilterOption("true");
                    }
                }

                if (item.Blighted)
                {
                    Query.Filters.MapFilters.Filters.Blighted = new SearchFilterOption("true");
                }
            }

            if (item.MapTier > 0)
            {
                Query.Filters.MapFilters.Filters.MapTier = new SearchFilterValue()
                {
                    Min = item.MapTier,
                    Max = item.MapTier,
                };
            }

            if (stats != null && stats.Count > 0)
            {
                Query.Stats = new List<StatFilterGroup>()
                {
                    new StatFilterGroup()
                    {
                        Type = StatType.And,
                        Filters = stats
                    }
                };
            }
        }

        public Query Query { get; set; } = new Query();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };

        private (double min, double max) ParseRange(string input)
        {
            var index = input.IndexOf("-");

            if (index < 0)
            {
                return (0, 0);
            }

            if (!double.TryParse(input.Substring(0, index), out var min))
            {
                min = 0;
            }

            if (!double.TryParse(input.Substring(index + 1, input.Length - index - 1), out var max))
            {
                max = 0;
            }

            return (min, max);
        }

        private double CalculateDps(double min, double max, double attackSpeed)
        {
            return Math.Round(((min + max) / 2) * attackSpeed, MidpointRounding.ToEven);
        }
    }
}
