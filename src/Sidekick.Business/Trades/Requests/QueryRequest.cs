using System;
using System.Collections.Generic;
using Sidekick.Business.Filters;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Trades.Requests
{
    public class QueryRequest
    {
        public QueryRequest(Parsers.Models.Item item, ILanguage language)
        {
            var itemType = item.GetType();

            if (itemType == typeof(EquippableItem))
            {
                if (((EquippableItem)item).Rarity == language.RarityUnique)
                {
                    Query.Name = item.Name;
                    Query.Filters.TypeFilter.Filters.Rarity = new FilterOption()
                    {
                        Option = "Unique",
                    };
                }
                else
                {
                    Query.Type = item.Type;
                    Query.Filters.TypeFilter.Filters.Rarity = new FilterOption()
                    {
                        Option = "nonunique",
                    };

                    if (!int.TryParse(((EquippableItem)item).ItemLevel, out var result))
                    {
                        throw new Exception("Couldn't parse Item Level");
                    }

                    if (result >= 86)
                    {
                        Query.Filters.MiscFilters.Filters.ItemLevel = new FilterValue()
                        {
                            Min = 86
                        };
                    }
                    else
                    {
                        Query.Filters.MiscFilters.Filters.ItemLevel = new FilterValue()
                        {
                            Min = result,
                            Max = result,
                        };
                    }

                    switch (((EquippableItem)item).Influence)
                    {
                        case InfluenceType.None:
                            break;
                        case InfluenceType.Shaper:
                            Query.Filters.MiscFilters.Filters.ShaperItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Crusader:
                            Query.Filters.MiscFilters.Filters.CrusaderItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Elder:
                            Query.Filters.MiscFilters.Filters.ElderItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Hunter:
                            Query.Filters.MiscFilters.Filters.HunterItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Redeemer:
                            Query.Filters.MiscFilters.Filters.RedeemerItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                        case InfluenceType.Warlord:
                            Query.Filters.MiscFilters.Filters.WarlordItem = new FilterOption()
                            {
                                Option = "true"
                            };
                            break;
                    }
                }

                if (((EquippableItem)item).Links != null)        // Auto Search 5+ Links
                {
                    Query.Filters.SocketFilter.Filters.Links = ((EquippableItem)item).Links;
                }
            }
            else if (itemType == typeof(OrganItem))
            {
                Query.Term = item.Name;
                Query.Filters.TypeFilter.Filters.Category = new FilterOption()
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

                Query.Filters.MiscFilters = new MiscFilter();
                Query.Filters.MiscFilters.Filters.GemLevel = new FilterValue()
                {
                    Min = result,
                    Max = result,
                };

                if (!int.TryParse(((GemItem)item).Quality, out result))
                {
                    throw new Exception("Unable to parse Gem Quality");
                }

                Query.Filters.MiscFilters.Filters.Quality = new FilterValue()
                {
                    Min = result,
                    Max = result,
                };

                Query.Filters.MiscFilters.Filters.Corrupted = new FilterOption()
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
                if (((MapItem)item).Rarity == language.RarityUnique)
                {
                    Query.Name = item.Name;
                    Query.Filters.TypeFilter.Filters.Rarity = new FilterOption()
                    {
                        Option = "Unique",
                    };
                }
                else
                {
                    Query.Type = item.Type;
                    Query.Filters.TypeFilter.Filters.Rarity = new FilterOption()
                    {
                        Option = "nonunique",
                    };
                }

                if (!int.TryParse(((MapItem)item).MapTier, out var result))
                {
                    throw new Exception("Unable to parse Map Tier");
                }

                Query.Filters.MapFilter.Filters.MapTier = new FilterValue()       // Search correct map tier
                {
                    Min = result,
                    Max = result,
                };

                Query.Filters.MapFilter.Filters.Blighted = new FilterOption()
                {
                    Option = ((MapItem)item).IsBlight,
                };
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
    }
}
