using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;

namespace Sidekick.Business.Parsers.Models
{
    public interface IAttributeItem
    {
        Dictionary<StatData, SearchFilterValue> AttributeDictionary { get; set; }
    }
}
