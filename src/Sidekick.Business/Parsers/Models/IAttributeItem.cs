using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Filters;

namespace Sidekick.Business.Parsers.Models
{
    public interface IAttributeItem
    {
        Dictionary<StatData, FilterValue> AttributeDictionary { get; set; }
    }
}
