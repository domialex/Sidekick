using System.Collections.Generic;

namespace Sidekick.Helpers.POETradeAPI.Models.TradeData
{
    /// <summary>
    /// Pseudo, Explicit, Implicit, etc.
    /// </summary>
    public class AttributeCategory
    {
        public string Label { get; set; }
        public List<Attribute> Entries { get; set; }
    }

    public class Attribute
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }
}
