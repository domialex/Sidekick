using System.Collections.Generic;

namespace Sidekick.Business.Trades.Models
{
    /// <summary>
    /// Pseudo, Explicit, Implicit, etc.
    /// </summary>
    public class AttributeCategory
    {
        public string Label { get; set; }
        public List<Attribute> Entries { get; set; }
    }
}
