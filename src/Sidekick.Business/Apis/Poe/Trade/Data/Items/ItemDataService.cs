using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemDataService : IItemDataService
    {
        public Dictionary<string, List<ItemData>> NameAndTypeDictionary { get; set; }
        public List<(Regex Regex, ItemData Item)> NameAndTypeRegex { get; set; }
        public string[] Prefixes { get; set; }

        public ItemData ParseItemData(ItemSections itemSections, Rarity itemRarity)
        {
            var results = new List<ItemData>();

            // Rares may have conflicting names, so we don't want to search any unique items that may have that name. Like "Ancient Orb" which can be used by abyss jewels.
            // There are some items which have prefixes which we don't want to remove, like the "Blighted Delirium Orb".
            if (itemRarity != Rarity.Rare && NameAndTypeDictionary.TryGetValue(itemSections.HeaderSection[1], out var itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemRarity != Rarity.Rare && NameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(itemSections.HeaderSection[1]), out itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemSections.HeaderSection.Length > 2 && NameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(itemSections.HeaderSection[2]), out itemData))
            {
                results.AddRange(itemData);
            }
            else
            {
                results.AddRange(NameAndTypeRegex
                    .Where(pattern => pattern.Regex.IsMatch(itemSections.WholeSections[0]))
                    .Select(x => x.Item));
            }

            if (results.Any(item => item.Rarity == Rarity.Gem)
                && itemSections.TryGetVaalGemName(out var vaalGemName)
                && NameAndTypeDictionary.TryGetValue(vaalGemName, out itemData))
            {
                // If we find a Vaal gem, we don't care about other matches
                results.Clear();
                results.Add(itemData.First());
            }

            return results
                .OrderBy(x => x.Rarity == Rarity.Unique ? 0 : 1)
                .ThenBy(x => x.Rarity == Rarity.Unknown ? 0 : 1)
                .ThenBy(x => x.Type == itemSections.HeaderSection.Last() ? 0 : 1)
                .FirstOrDefault();
        }

        private string GetLineWithoutPrefixes(string line)
        {
            foreach (var prefix in Prefixes)
            {
                line = line.Replace(prefix, "");
            }

            return line.Trim();
        }
    }
}
