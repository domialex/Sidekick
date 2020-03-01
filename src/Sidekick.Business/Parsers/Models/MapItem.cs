using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Filters;

namespace Sidekick.Business.Parsers.Models
{
    public class MapItem : Item, IAttributeItem
    {
        public string MapTier { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemRarity { get; set; }
        public string MonsterPackSize { get; set; }
        public string IsBlight { get; set; }
        public Dictionary<Attribute, FilterValue> AttributeDictionary { get; set; }
    }
}
