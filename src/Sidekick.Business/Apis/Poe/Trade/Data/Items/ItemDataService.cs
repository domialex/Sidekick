using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemDataService : IItemDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;

        public ItemDataService(IPoeTradeClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        private Dictionary<string, List<ItemData>> Patterns { get; set; }

        public async Task OnInit()
        {
            Patterns = new Dictionary<string, List<ItemData>>();

            var categories = await poeApiClient.Fetch<ItemDataCategory>();

            FillPattern(categories[0].Entries); // Accessories
            FillPattern(categories[1].Entries); // Armour
            FillPattern(categories[2].Entries, Rarity.DivinationCard); // Cards
            FillPattern(categories[3].Entries, Rarity.Currency); // Currency
            FillPattern(categories[4].Entries); // Flasks
            FillPattern(categories[5].Entries, Rarity.Gem); // Gems
            FillPattern(categories[6].Entries); // Jewels
            FillPattern(categories[7].Entries); // Maps
            FillPattern(categories[8].Entries); // Weapons
            FillPattern(categories[9].Entries); // Leaguestones
            FillPattern(categories[10].Entries, Rarity.Prophecy); // Prophecies
            FillPattern(categories[11].Entries); // Itemised Monsters
            FillPattern(categories[12].Entries); // Watchstones
        }

        private void FillPattern(List<ItemData> items, Rarity? rarity = null)
        {
            foreach (var item in items)
            {
                item.Rarity = Rarity.Unknown;
                if (item.Flags.Unique)
                {
                    item.Rarity = Rarity.Unique;
                }
                else if (item.Flags.Prophecy)
                {
                    item.Rarity = Rarity.Prophecy;
                }
                else if (rarity.HasValue)
                {
                    item.Rarity = rarity.Value;
                }

                var key = item.Name ?? item.Type;

                if (!Patterns.TryGetValue(key, out var itemData))
                {
                    itemData = new List<ItemData>();
                    Patterns.Add(key, itemData);
                }

                itemData.Add(item);
            }
        }

        public ItemData ParseItemData(ItemSections itemText)
        {
            var results = new List<ItemData>();

            if (Patterns.TryGetValue(itemText.HeaderSection[1], out var itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemText.HeaderSection.Length > 2 && Patterns.TryGetValue(itemText.HeaderSection[2], out itemData))
            {
                results.AddRange(itemData);
            }

            if(results.Any(item => item.Rarity == Rarity.Gem)
                && itemText.TryGetVaalGemName(out var vaalGemName)
                && Patterns.TryGetValue(vaalGemName, out itemData))
            {
                // If we find a Vaal gem, we don't care about other matches
                results.Clear();
                results.Add(itemData.First());
            }

            return results
                .OrderBy(x => x.Rarity == Rarity.Unique ? 0 : 1)
                .ThenBy(x => x.Rarity == Rarity.Unknown ? 0 : 1)
                .FirstOrDefault();
        }
    }
}
