using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Filters;

namespace Sidekick.Business.Parsers.Models
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsCorrupted { get; set; }
        public bool IsIdentified { get; set; }
        public string Rarity { get; set; }
        public string ItemText { get; set; }
    }
}
