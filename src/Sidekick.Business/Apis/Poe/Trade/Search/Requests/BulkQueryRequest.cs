using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Requests
{
    public class BulkQueryRequest
    {
        public BulkQueryRequest(Item item, IStaticDataService staticDataService)
        {
            Exchange.Status.Option = StatusType.Online;

            Exchange.Want.Add(staticDataService.GetId(item));
            Exchange.Have.Add("chaos");
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
