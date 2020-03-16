using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;

namespace Sidekick.Business.Trades.Requests
{
    public class Query
    {
        public Status Status { get; set; } = new Status();
        public string Name { get; set; }
        public string Type { get; set; }
        public string Term { get; set; }
        public List<StatFilterGroup> Stats { get; set; } = new List<StatFilterGroup>();
        public Filters Filters { get; set; } = new Filters();
    }
}
