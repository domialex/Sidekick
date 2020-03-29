using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public interface IItemDataService
    {
        ItemData ParseItemData(ItemTextBlock itemText);
    }
}
