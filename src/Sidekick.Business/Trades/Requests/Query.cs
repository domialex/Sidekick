using Sidekick.Business.Filters;
using System.Collections.Generic;

namespace Sidekick.Business.Trades.Requests
{
    public class Query
    {
        public Status Status { get; set; } = new Status();
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Stat> Stats { get; set; } = new List<Stat>();
        public Filters.Filters Filters { get; set; } = new Filters.Filters();
    }
}
