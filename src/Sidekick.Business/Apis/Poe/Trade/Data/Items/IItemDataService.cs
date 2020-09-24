using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public interface IItemDataService
    {
        ItemData ParseItemData(ItemSections itemText, Rarity itemRarity);
    }
}
