using System.Collections.Generic;

namespace Sidekick.Helpers.POETradeAPI.Models
{
    /// <summary>
    /// Currencies, Fragments, Maps, etc.
    /// </summary>
    public class StaticItemCategory
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<StaticItem> Entries { get; set; }
    }

    public class StaticItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
    }
}
