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
            if (item.Links >= 5)
            {
                Query.filters.socket_filters.disabled = false;
                Query.filters.socket_filters.filters.links.min = item.Links;
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
        public Filters filters { get; set; } = new Filters();
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
    public class Filters
    {
        public Weapon_filters weapon_filters { get; set; }
        public Armour_filters armour_filters { get; set; }
        public Socket_filters socket_filters { get; set; } = new Socket_filters();
        public Req_filters req_filters { get; set; }
        public Misc_filters misc_filters { get; set; }
        public Trade_filters trade_filters { get; set; }
        public Type_filters type_filters { get; set; }
        public class Weapon_filters
        {
            //TODO
        }
        public class Armour_filters
        {
            //TODO
        }
        public class Socket_filters
        {
            public bool disabled { get; set; } = true;
            public Filters filters { get; set; } = new Filters();
            public class Filters
            {
                public Sockets sockets { get; set; } = new Sockets();
                public Links links { get; set; } = new Links();
                public class Sockets
                {
                    public int? min { get; set; } = null;
                    public int? max { get; set; } = null;
                    public int? r { get; set; } = null;
                    public int? g { get; set; } = null;
                    public int? b { get; set; } = null;
                    public int? w { get; set; } = null;
                }
                public class Links
                {
                    public int? min { get; set; } = null;
                    public int? max { get; set; } = null;
                    public int? r { get; set; } = null;
                    public int? g { get; set; } = null;
                    public int? b { get; set; } = null;
                    public int? w { get; set; } = null;
                }
            }
        }
        public class Req_filters
        {
            //TODO
        }
        public class Misc_filters
        {
            //TODO
        }
        public class Trade_filters
        {
            //TODO
        }
        public class Type_filters
        {
            //TODO
        }
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
