using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Requests
{
    public class BulkQueryRequest
    {
        public BulkQueryRequest(Item item, ILanguage language, IStaticDataService staticDataService)
        {
            Exchange.Status.Option = StatusType.Online;

            Exchange.Want.Add(staticDataService.Lookup[item.Type]);
            Exchange.Have.Add("chaos"); // TODO: Add support for other currency types?
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
