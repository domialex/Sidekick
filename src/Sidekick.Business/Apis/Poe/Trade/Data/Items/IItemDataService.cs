using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public interface IItemDataService
    {
        Dictionary<string, List<ItemData>> NameAndTypeDictionary { get; set; }
        List<(Regex Regex, ItemData Item)> NameAndTypeRegex { get; set; }
        string[] Prefixes { get; set; }
        ItemData ParseItemData(ItemSections itemSections, Rarity itemRarity);
    }
}
