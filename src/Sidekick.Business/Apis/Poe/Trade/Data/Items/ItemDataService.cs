using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
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

        private List<(Regex Regex, ItemData Item)> Patterns { get; set; }

        private Regex NewlinePattern;

        public async Task OnInit()
        {
            Patterns = new List<(Regex Regex, ItemData Item)>();

            var categories = await poeApiClient.Fetch<ItemDataCategory>();

            FillPattern(null, categories[0].Entries); // Accessories
            FillPattern(null, categories[1].Entries); // Armour
            FillPattern(Rarity.DivinationCard, categories[2].Entries); // Cards
            FillPattern(Rarity.Currency, categories[3].Entries); // Currency
            FillPattern(null, categories[4].Entries); // Flasks
            FillPattern(Rarity.Gem, categories[5].Entries); // Gems
            FillPattern(null, categories[6].Entries); // Jewels
            FillPattern(null, categories[7].Entries); // Maps
            FillPattern(null, categories[8].Entries); // Weapons
            FillPattern(null, categories[9].Entries); // Leaguestones
            FillPattern(Rarity.Prophecy, categories[10].Entries); // Prophecies
            FillPattern(null, categories[11].Entries); // Itemised Monsters
            FillPattern(null, categories[12].Entries); // Watchstones

            NewlinePattern = new Regex("[\\r\\n]+");
        }

        private void FillPattern(Rarity? rarity, List<ItemData> items)
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

                Regex regex;
                if (string.IsNullOrEmpty(item.Name))
                {
                    if (item.Rarity == Rarity.DivinationCard || item.Rarity == Rarity.Gem)
                    {
                        regex = new Regex($"^{Regex.Escape(item.Type)}$");
                    }
                    else
                    {
                        regex = new Regex(Regex.Escape(item.Type));
                    }
                }
                else
                {
                    regex = new Regex($"^{Regex.Escape(item.Name)}$");
                }

                Patterns.Add((regex, item));
            }
        }

        public ItemData ParseItem(string text)
        {
            var lines = NewlinePattern.Split(text);
            var results = new List<ItemData>();

            if (lines.Length >= 2)
            {
                results.AddRange(Patterns
                    .Where(x => x.Regex.IsMatch(lines[1]))
                    .Select(x => x.Item)
                    .ToList());
            }

            if (lines.Length >= 3)
            {
                results.AddRange(Patterns
                    .Where(x => x.Regex.IsMatch(lines[2]))
                    .Select(x => x.Item)
                    .ToList());
            }

            // We need to check for vaal gems
            if (results.Any(x => x.Rarity == Rarity.Gem))
            {
                foreach (var line in lines.Skip(3))
                {
                    results.AddRange(Patterns
                        .Where(x => x.Regex.IsMatch(line))
                        .Select(x => x.Item)
                        .ToList());
                }
            }

            return results
                .OrderBy(x => x.Rarity == Rarity.Unique || x.Rarity == Rarity.DivinationCard ? 0 : 1)
                .ThenBy(x => x.Rarity == Rarity.Unknown ? 0 : 1)
                .ThenByDescending(x => x.Rarity == Rarity.Gem ? x.Text.Length : 0)
                .FirstOrDefault();
        }
    }
}
