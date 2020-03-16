using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Parsers.Models
{
    public class EquippableItem : Item, IAttributeItem
    {
        public string Quality { get; set; }
        public string ItemLevel { get; set; }
        public InfluenceType Influence { get; set; }
        public int MaxSockets { get; set; }
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
        public Dictionary<StatData, FilterValue> AttributeDictionary { get; set; }
    }
}
