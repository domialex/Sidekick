using System.Collections.Generic;
using System.Text;
using Sidekick.Business.Filters;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Parsers.Models
{
    public interface IAttributeItem
    {
        Dictionary<Attribute, FilterValue> AttributeDictionary { get; set; }
    }
}
