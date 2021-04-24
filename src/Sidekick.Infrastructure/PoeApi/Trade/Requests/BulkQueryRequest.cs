using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;

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
