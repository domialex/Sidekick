using System.Collections.Generic;

namespace Sidekick.Business.Categories.Models
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
}
