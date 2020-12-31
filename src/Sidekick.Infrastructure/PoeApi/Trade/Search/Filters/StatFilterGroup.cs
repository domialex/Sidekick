using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeApi.Trade.Search.Filters
{
    public class StatFilterGroup
    {
        public StatType Type { get; set; }
        public List<StatFilter> Filters { get; set; }
    }
}
