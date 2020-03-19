using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    /// <summary>
    /// Pseudo, Explicit, Implicit, etc.
    /// </summary>
    public class StatDataCategory
    {
        public string Label { get; set; }

        public List<StatData> Entries { get; set; }

    }
}
