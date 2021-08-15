using System.Collections.Generic;
using Sidekick.Apis.Poe;
using Sidekick.Common.Game.Items;

namespace Sidekick.Infrastructure.PoeApi.Trade.Requests
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
