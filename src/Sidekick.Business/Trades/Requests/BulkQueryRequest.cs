using System.Collections.Generic;
using Sidekick.Business.Categories;
using Sidekick.Business.Filters;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Trades.Requests
{
    public class BulkQueryRequest
    {
        public BulkQueryRequest(Item item, ILanguage language, IStaticDataService staticItemCategoryService)
        {
            Exchange.Status.Option = StatusType.Online;

            Exchange.Want.Add(staticItemCategoryService.Lookup[item.Type]);
            Exchange.Have.Add("chaos"); // TODO: Add support for other currency types?
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
