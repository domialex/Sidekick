using Newtonsoft.Json;
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
                if(((EquippableItem)item).Rarity == StringConstants.RarityUnique)
                {
                    Query.Name = item.Name;
                }
                else
                {
                    Query.Type = item.Type;

                    if (!int.TryParse(((EquippableItem)item).ItemLevel, out var result))
                    {
                        throw new Exception("Couldn't parse Item Level");
                    }

                    Query.Filters.MiscFilters.Filters.ItemLevel = new FilterValue()
                    {
                        Min = result,
                        Max = result,
                    };

                    Query.Filters.TypeFilter.Filters.Rarity = new FilterOption()
                    {
                        Option = ((EquippableItem)item).Rarity.ToLowerInvariant(),
                    };
                }             

                if(((EquippableItem)item).Links != null)        // Auto Search 5+ Links
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
                var itemId = TradeClient.StaticItemCategories.Single(x => x.Id == "Currency")
                                                               .Entries
                                                               .Single(x => x.Text == item.Name)
                                                               .Id;

                Exchange.Want.Add(itemId);
                Exchange.Have.Add("chaos"); // TODO: Add support for other currency types?
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

    public class Exchange
    {
        public List<string> Want { get; set; } = new List<string>();
        public List<string> Have { get; set; } = new List<string>();
        public string Status = "online";
    }

    public class Status
    {
        public StatusType Option { get; set; }
    }

    public class Stat
    {
        public StatType Type { get; set; }
        public List<StatFilter> Filters { get; set; }
    }

    public class StatFilter
    {
        public string Id { get; set; }
        public FilterValue Value { get; set; }
        public bool Disabled { get; set; }
    }

    public class Filters
    {
        [JsonProperty(PropertyName = "misc_filters")]
        public MiscFilter MiscFilters { get; set; } = new MiscFilter();
        [JsonProperty(PropertyName = "weapon_filters")]
        public WeaponFilter WeaponFilters { get; set; } = new WeaponFilter();
        [JsonProperty(PropertyName = "armour_filters")]
        public ArmourFilter ArmourFilter { get; set; } = new ArmourFilter();
        [JsonProperty(PropertyName = "socket_filters")]
        public SocketFilter SocketFilter { get; set; } = new SocketFilter();
        [JsonProperty(PropertyName = "req_filters")]
        public RequierementFilter RequierementFilter { get; set; } = new RequierementFilter();
        [JsonProperty(PropertyName = "type_filters")]
        public TypeFilter TypeFilter { get; set; } = new TypeFilter();
    }

    public class MiscFilter
    {
        public bool Disabled { get; set; }
        public MiscFilters Filters { get; set; } = new MiscFilters();
    }

    public class WeaponFilter
    {
        public bool Disabled { get; set; }
        public WeaponFilters Filters { get; set; } = new WeaponFilters();
    }

    public class ArmourFilter
    {
        public bool Disabled { get; set; }
        public ArmorFilters Filters { get; set; } = new ArmorFilters();
    }

    public class SocketFilter
    {
        public bool Disabled { get; set; }
        public SocketFilters Filters { get; set; } = new SocketFilters();
    }

    public class RequierementFilter
    {
        public bool Disabled { get; set; }
        public RequierementFilters Filters { get; set; } = new RequierementFilters();
    }

    public class TypeFilter
    {
        public bool Disabled { get; set; }
        public TypeFilters Filters { get; set; } = new TypeFilters();
    }

    public class MiscFilters
    {
        public FilterValue Quality { get; set; }
        [JsonProperty(PropertyName = "map_tier")]
        public FilterValue MapTier { get; set; }
        [JsonProperty(PropertyName = "map_iiq")]
        public FilterValue MapIiq { get; set; }
        [JsonProperty(PropertyName = "gem_level")]
        public FilterValue GemLevel { get; set; }
        [JsonProperty(PropertyName = "ilvl")]
        public FilterValue ItemLevel { get; set; }
        [JsonProperty(PropertyName = "map_packsize")]
        public FilterValue MapPacksize { get; set; }
        [JsonProperty(PropertyName = "map_iir")]
        public FilterValue MapIir { get; set; }
        [JsonProperty(PropertyName = "talisman_art")]
        public FilterOption TalismanArt { get; set; }
        [JsonProperty(PropertyName = "alternate_art")]
        public FilterOption AlternateArt { get; set; }
        public FilterOption Identified { get; set; }
        public FilterOption Corrupted { get; set; }
        public FilterOption Crafted { get; set; }
        public FilterOption Enchanted { get; set; }
    }

    public class WeaponFilters
    {
        public FilterValue Damage { get; set; }
        public FilterValue Crit { get; set; }
        public FilterValue APS { get; set; }
        public FilterValue EDPS { get; set; }
        public FilterValue PDPS { get; set; }
    }

    public class ArmorFilters
    {
        [JsonProperty(PropertyName = "ar")]
        public FilterValue Armor { get; set; }
        [JsonProperty(PropertyName = "es")]
        public FilterValue EnergyShield { get; set; }
        [JsonProperty(PropertyName = "ev")]
        public FilterValue Evasion { get; set; }
        [JsonProperty(PropertyName = "block")]
        public FilterValue Block { get; set; }
    }

    public class SocketFilters
    {
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
    }

    public class RequierementFilters
    {
        [JsonProperty(PropertyName = "lvl")]
        public FilterValue Level { get; set; }
        [JsonProperty(PropertyName = "dex")]
        public FilterValue Dexterity { get; set; }
        [JsonProperty(PropertyName = "str")]
        public FilterValue Strength { get; set; }
        [JsonProperty(PropertyName = "int")]
        public FilterValue Intelligence { get; set; }
    }

    public class TypeFilters
    {
        public FilterOption Category { get; set; }
        public FilterOption Rarity { get; set; }
    }

    public class FilterValue
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
    }

    public class FilterOption
    {
        public string Option { get; set; }
    }

    public class SocketFilterOption : FilterValue
    {
        [JsonProperty(PropertyName = "r")]
        public int? Red { get; set; }
        [JsonProperty(PropertyName = "g")]
        public int? Green { get; set; }
        [JsonProperty(PropertyName = "b")]
        public int? Blue { get; set; }
        [JsonProperty(PropertyName = "w")]
        public int? White { get; set; }
    }

    public enum SortType
    {
        Asc,
        Desc
    }

    public enum StatusType
    {
        Online,
        Any
    }

    public enum StatType
    {
        And,
        Or,
        Count
    }

    public enum InfluenceType
    {
        None,
        Shaper,
        Elder,
        Crusader,
        Hunter,
        Redeemer,
        Warlord,
    };
}
