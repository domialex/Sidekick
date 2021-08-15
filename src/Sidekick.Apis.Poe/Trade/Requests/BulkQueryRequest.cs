using System.Collections.Generic;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.Poe.Trade.Requests
{
    public class BulkQueryRequest
    {
        public BulkQueryRequest(Item item, IItemStaticDataProvider staticDataProvider)
        {
            Exchange.Status.Option = StatusType.Online;

            Exchange.Want.Add(staticDataProvider.GetId(item));
            Exchange.Have.Add("chaos");
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
