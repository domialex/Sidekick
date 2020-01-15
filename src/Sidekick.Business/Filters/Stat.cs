using Sidekick.Business.Types;
using System.Collections.Generic;

namespace Sidekick.Business.Filters
{
    public class Stat
    {
        public StatType Type { get; set; }
        public List<StatFilter> Filters { get; set; }
    }
}
