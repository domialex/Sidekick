using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public interface IItemDataService
    {
        List<ItemCategory> Categories { get; }
        Item GetItem(string name);
    }
}
