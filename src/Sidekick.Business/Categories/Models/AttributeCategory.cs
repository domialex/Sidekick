using System.Collections.Generic;

namespace Sidekick.Business.Categories.Models
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
