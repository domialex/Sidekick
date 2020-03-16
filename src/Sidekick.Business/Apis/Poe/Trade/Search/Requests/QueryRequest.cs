using System;
using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Requests
{
    public class QueryRequest
    {
        public QueryRequest(Parsers.Models.Item item)
        {
            Query.Status.Option = StatusType.Online;
            Query.Filters.TradeFilters.Filters.SaleType = new SearchFilterOption { Option = "priced" };

            var itemType = item.GetType();

            if (itemType == typeof(EquippableItem))
            {
                if (((EquippableItem)item).Rarity == Rarity.Unique)
                {
                    Query.Name = item.Name;
                    Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "Unique",
                    };
                }
                else
                {
                    Query.Type = item.Type;
                    Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "nonunique",
                    };

                    if (!int.TryParse(((EquippableItem)item).ItemLevel, out var result))
                    {
                        throw new Exception("Couldn't parse Item Level");
                    }

                    if (result >= 86)
                    {
                        Query.Filters.MiscFilters.Filters.ItemLevel = new SearchFilterValue()
                        {
                            Min = 86
                        };
                    }
                    //else
                    //{
                    //    Query.Filters.MiscFilters.Filters.ItemLevel = new FilterValue()
                    //    {
                    //        Min = result,
                    //        Max = result,
                    //    };
                    //}

                    switch (((EquippableItem)item).Influence)
                    {
                        case InfluenceType.None:
                            break;
                        case InfluenceType.Shaper:
                            Query.Filters.MiscFilters.Filters.ShaperItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Crusader:
                            Query.Filters.MiscFilters.Filters.CrusaderItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Elder:
                            Query.Filters.MiscFilters.Filters.ElderItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Hunter:
                            Query.Filters.MiscFilters.Filters.HunterItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Redeemer:
                            Query.Filters.MiscFilters.Filters.RedeemerItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Warlord:
                            Query.Filters.MiscFilters.Filters.WarlordItem = new SearchFilterOption()
                            {
                                Option = "true"
                            };
                            break;
                    }
                }

                if (((EquippableItem)item).Links != null)        // Auto Search 5+ Links
                {
                    Query.Filters.SocketFilters.Filters.Links = ((EquippableItem)item).Links;
                }

                if (((EquippableItem)item).AttributeDictionary != null)
                {
                    var statFilters = new List<StatFilter>();

                    foreach (var pair in ((EquippableItem)item).AttributeDictionary)
                    {
                        statFilters.Add(new StatFilter()
                        {
                            Disabled = false,
                            Id = pair.Key.Id,
                            Value = pair.Value,
                        });
                    }

                    Query.Stats = new List<StatFilterGroup>() { new StatFilterGroup() { Type = StatType.And, Filters = statFilters } };
                }

                // TODO Block Chance
                if (itemType == typeof(ArmourItem))
                {
                    if (int.TryParse(((ArmourItem)item).Armour, out var armor))
                    {
                        Query.Filters.ArmourFilters.Filters.Armor = new SearchFilterValue() { Min = armor };
                    }

                    if (int.TryParse(((ArmourItem)item).EnergyShield, out var es))
                    {
                        Query.Filters.ArmourFilters.Filters.EnergyShield = new SearchFilterValue() { Min = es };
                    }

                    if (int.TryParse(((ArmourItem)item).Evasion, out var evasion))
                    {
                        Query.Filters.ArmourFilters.Filters.Evasion = new SearchFilterValue() { Min = evasion };
                    }
                }
                else if (itemType == typeof(WeaponItem))
                {
                    var physDamage = ParseRange(((WeaponItem)item).PhysicalDamage);
                    var elementalDamage = ParseRange(((WeaponItem)item).ElementalDamage);

                    if (!double.TryParse(((WeaponItem)item).AttacksPerSecond, out var attackSpeed))
                    {
                        attackSpeed = 0;
                    }

                    var pdps = CalculateDps(physDamage.min, physDamage.max, attackSpeed);
                    var edps = CalculateDps(elementalDamage.min, elementalDamage.max, attackSpeed);

                    if (!double.TryParse(((WeaponItem)item).CriticalStrikeChance, out var critChance))
                    {
                        critChance = 0;
                    }

                    Query.Filters.WeaponFilters.Filters.APS = new SearchFilterValue() { Min = attackSpeed };
                    Query.Filters.WeaponFilters.Filters.Crit = new SearchFilterValue() { Min = critChance };
                    Query.Filters.WeaponFilters.Filters.EDPS = new SearchFilterValue() { Min = edps };
                    Query.Filters.WeaponFilters.Filters.PDPS = new SearchFilterValue() { Min = pdps };
                }
            }
            else if (itemType == typeof(OrganItem))
            {
                Query.Term = item.Name;
                Query.Filters.TypeFilters.Filters.Category = new SearchFilterOption()
                {
                    Option = "monster.sample"
                };
            }
            else if (itemType == typeof(CurrencyItem))
            {
                Query.Type = item.Name;
            }
            else if (itemType == typeof(GemItem))
            {
                Query.Type = item.Type;

                if (!int.TryParse(((GemItem)item).Level, out var result))
                {
                    throw new Exception("Unable to parse Gem Level");
                }

                Query.Filters.MiscFilters = new MiscFilterGroup();
                Query.Filters.MiscFilters.Filters.GemLevel = new SearchFilterValue()
                {
                    Min = result,
                    Max = result,
                };

                if (!int.TryParse(((GemItem)item).Quality, out result))
                {
                    throw new Exception("Unable to parse Gem Quality");
                }

                Query.Filters.MiscFilters.Filters.Quality = new SearchFilterValue()
                {
                    Min = result,
                    Max = result,
                };

                Query.Filters.MiscFilters.Filters.Corrupted = new SearchFilterOption()
                {
                    Option = item.IsCorrupted ? "true" : "false"
                };
            }
            else if (itemType == typeof(FragmentItem))
            {
                Query.Type = item.Type;
            }
            else if (itemType == typeof(MapItem))
            {
                if (((MapItem)item).Rarity == Rarity.Unique)
                {
                    Query.Name = item.Name;
                    Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "Unique",
                    };
                }
                else
                {
                    Query.Type = item.Type;
                    Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption()
                    {
                        Option = "nonunique",
                    };
                }

                if (!int.TryParse(((MapItem)item).MapTier, out var result))
                {
                    throw new Exception("Unable to parse Map Tier");
                }

                Query.Filters.MapFilters.Filters.MapTier = new SearchFilterValue()       // Search correct map tier
                {
                    Min = result,
                    Max = result,
                };

                Query.Filters.MapFilters.Filters.Blighted = new SearchFilterOption()
                {
                    Option = ((MapItem)item).IsBlight,
                };

                if (((MapItem)item).AttributeDictionary != null)
                {
                    var statFilters = new List<StatFilter>();

                    foreach (var pair in ((MapItem)item).AttributeDictionary)
                    {
                        statFilters.Add(new StatFilter()
                        {
                            Disabled = false,
                            Id = pair.Key.Id,
                            Value = pair.Value,
                        });

                        Query.Stats = new List<StatFilterGroup>() { new StatFilterGroup() { Type = StatType.And, Filters = statFilters } };
                    }
                }
            }
            else if (itemType == typeof(ProphecyItem))
            {
                Query.Name = item.Name;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Query Query { get; set; } = new Query();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };

        private (double min, double max) ParseRange(string input)
        {
            int index = input.IndexOf("-");

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
