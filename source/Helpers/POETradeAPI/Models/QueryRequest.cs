using Newtonsoft.Json;
using Sidekick.Business.Filters;
using Sidekick.Business.Items;
using Sidekick.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Helpers.POETradeAPI.Models
{
    public class QueryRequest
    {
        public QueryRequest()
        {

        }

        public QueryRequest(Item item)
        {
            var itemType = item.GetType();

            if (itemType == typeof(EquippableItem))
            {
                if (((EquippableItem)item).Rarity == LanguageSettings.Provider.RarityUnique)
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
                    Option = item.IsCorrupted
                };
            }
            else if (itemType == typeof(FragmentItem))
            {
                Query.Type = item.Type;
            }
            else if (itemType == typeof(MapItem))
            {
                if (((MapItem)item).Rarity == LanguageSettings.Provider.RarityUnique)
                {
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

    public class BulkQueryRequest
    {

        public BulkQueryRequest()
        {

        }

        public BulkQueryRequest(Item item)
        {
            var itemType = item.GetType();

            if (itemType == typeof(CurrencyItem))
            {
                var itemCategory = "Currency";

                if (item.Name.Contains(LanguageSettings.Provider.KeywordCatalyst))
                {
                    itemCategory = "Catalysts";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordOil))
                {
                    itemCategory = "Oils";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordIncubator))
                {
                    itemCategory = "Incubators";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordScarab))
                {
                    itemCategory = "Scarabs";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordResonator))
                {
                    itemCategory = "DelveResonators";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordFossil))
                {
                    itemCategory = "DelveFossils";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordVial))
                {
                    itemCategory = "Vials";
                }
                else if (item.Name.Contains(LanguageSettings.Provider.KeywordEssence))
                {
                    itemCategory = "Essences";
                }

                var itemId = TradeClient.StaticItemCategories.Single(x => x.Id == itemCategory)
                                                               .Entries
                                                               .Single(x => x.Text == item.Name)
                                                               .Id;

                Exchange.Want.Add(itemId);
                Exchange.Have.Add("chaos"); // TODO: Add support for other currency types?
            }
            else if (itemType == typeof(DivinationCardItem))
            {
                var itemId = TradeClient.StaticItemCategories.Where(c => c.Id == "Cards").FirstOrDefault().Entries.Where(c => c.Text == item.Name).FirstOrDefault().Id;
                Exchange.Want.Add(itemId);
                Exchange.Have.Add("chaos");
            }
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }

    public class Query
    {
        public Status Status { get; set; } = new Status();
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Stat> Stats { get; set; } = new List<Stat>();
        public Filters Filters { get; set; } = new Filters();
    }

    

    
}
