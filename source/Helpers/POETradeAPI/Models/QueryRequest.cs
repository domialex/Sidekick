using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
                Query.Name = item.Name;
            }
            else if (itemType == typeof(CurrencyItem))
            {
                Query.Name = item.Name;
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
            else
            {
                throw new NotImplementedException();
            }
        }

        public Query Query { get; set; } = new Query();
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
    }

    public class MiscFilter
    {
        public bool Disabled { get; set; }
        public MiscFilters Filters { get; set; } = new MiscFilters();
    }

    public class MiscFilters
    {
        public FilterValue Quality { get; set; }
        [JsonProperty(PropertyName = "gem_level")]
        public FilterValue GemLevel { get; set; }
        public FilterOption Corrupted { get; set; }
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
}
