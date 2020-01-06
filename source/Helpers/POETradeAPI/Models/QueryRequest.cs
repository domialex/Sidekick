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
            switch (item.Rarity)
            {
                case "Unique":
                case "Currency":
                    Query.Name = item.Name;
                    break;
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

        // TODO: Implement Filters.

    }

    public class Status
    {
        public StatusType Option { get; set; }
    }

    public class Stat
    {
        public StatType Type { get; set; }
        public List<Filter> Filters { get; set; }

    }

    public class Filter
    {
        public string Id { get; set; }
        public FilterValue Value { get; set; }
        public bool Disabled { get; set; }

    }
    public class FilterValue
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
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
