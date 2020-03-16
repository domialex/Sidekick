using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;

namespace Sidekick.Business.Parsers.Models
{
    public class MapItem : Item, IAttributeItem
    {
        public string MapTier { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemRarity { get; set; }
        public string MonsterPackSize { get; set; }
        public string IsBlight { get; set; }
        public Dictionary<StatData, SearchFilterValue> AttributeDictionary { get; set; }
    }
}
