using System.Collections.Generic;
using Sidekick.Business.Filters;

namespace Sidekick.Business.Trades.Requests
{
    public class Query
    {
        public Status Status { get; set; } = new Status();
        public string Name { get; set; }
        public string Type { get; set; }
        public string Term { get; set; }
        public List<Stat> Stats { get; set; } = new List<Stat>();
        public Filters.Filters Filters { get; set; } = new Filters.Filters();
    }
}
