using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Caches;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemDataService : IItemDataService, IOnInit, IOnAfterInit
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly ILanguageProvider languageProvider;
        private readonly ICacheService cacheService;
        private Dictionary<string, List<ItemData>> nameAndTypeDictionary;
        private List<(Regex Regex, ItemData Item)> nameAndTypeRegex;

        private string[] prefixes;

        public ItemDataService(IPoeTradeClient poeApiClient,
            ILanguageProvider languageProvider,
            ICacheService cacheService)
        {
            this.poeApiClient = poeApiClient;
            this.languageProvider = languageProvider;
            this.cacheService = cacheService;
        }

        public async Task OnInit()
        {
            nameAndTypeDictionary = new Dictionary<string, List<ItemData>>();
            nameAndTypeRegex = new List<(Regex Regex, ItemData Item)>();

            var categories = await cacheService.GetOrCreate("ItemDataService.OnInit", () => poeApiClient.Fetch<ItemDataCategory>());

            FillPattern(categories[0].Entries, Category.Accessory, useRegex: true);
            FillPattern(categories[1].Entries, Category.Armour, useRegex: true);
            FillPattern(categories[2].Entries, Category.DivinationCard);
            FillPattern(categories[3].Entries, Category.Currency);
            FillPattern(categories[4].Entries, Category.Flask, useRegex: true);
            FillPattern(categories[5].Entries, Category.Gem);
            FillPattern(categories[6].Entries, Category.Jewel, useRegex: true);
            FillPattern(categories[7].Entries, Category.Map, useRegex: true);
            FillPattern(categories[8].Entries, Category.Weapon, useRegex: true);
            FillPattern(categories[9].Entries, Category.Leaguestone);
            FillPattern(categories[10].Entries, Category.Prophecy);
            FillPattern(categories[11].Entries, Category.ItemisedMonster, useRegex: true);
            FillPattern(categories[12].Entries, Category.Watchstone);
        }

        public Task OnAfterInit()
        {
            prefixes = new[]
            {
                languageProvider.Language.PrefixSuperior,
                languageProvider.Language.PrefixBlighted,
                languageProvider.Language.PrefixAnomalous,
                languageProvider.Language.PrefixDivergent,
                languageProvider.Language.PrefixPhantasmal,
            };

            return Task.CompletedTask;
        }

        private void FillPattern(List<ItemData> items, Category category, bool useRegex = false)
        {
            foreach (var item in items)
            {
                item.Rarity = GetRarityForCategory(category, item);
                item.Category = category;

                var key = item.Name ?? item.Type;

                if (useRegex)
                {
                    nameAndTypeRegex.Add((key.ToRegex(), item));
                }

                if (!nameAndTypeDictionary.TryGetValue(key, out var itemData))
                {
                    itemData = new List<ItemData>();
                    nameAndTypeDictionary.Add(key, itemData);
                }

                itemData.Add(item);
            }
        }

        private Rarity GetRarityForCategory(Category category, ItemData item)
        {
            if (item.Flags.Unique)
            {
                return Rarity.Unique;
            }
            else if (item.Flags.Prophecy)
            {
                return Rarity.Prophecy;
            }

            return category switch
            {
                Category.DivinationCard => Rarity.DivinationCard,
                Category.Gem => Rarity.Gem,
                Category.Currency => Rarity.Currency,
                _ => Rarity.Unknown
            };
        }

        public ItemData ParseItemData(ItemSections itemSections, Rarity itemRarity)
        {
            var results = new List<ItemData>();

            // Rares may have conflicting names, so we don't want to search any unique items that may have that name. Like "Ancient Orb" which can be used by abyss jewels.
            // there are some items which have prefixes which we don't wan to remove, like the "Blighted Delirium Orb"
            if (itemRarity != Rarity.Rare && nameAndTypeDictionary.TryGetValue(itemSections.HeaderSection[1], out var itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemRarity != Rarity.Rare && nameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(itemSections.HeaderSection[1]), out itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemSections.HeaderSection.Length > 2 && nameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(itemSections.HeaderSection[2]), out itemData))
            {
                results.AddRange(itemData);
            }
            else
            {
                results.AddRange(nameAndTypeRegex
                    .Where(pattern => pattern.Regex.IsMatch(itemSections.WholeSections[0]))
                    .Select(x => x.Item));
            }

            if (results.Any(item => item.Rarity == Rarity.Gem)
                && itemSections.TryGetVaalGemName(out var vaalGemName)
                && nameAndTypeDictionary.TryGetValue(vaalGemName, out itemData))
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
            foreach (var prefix in prefixes)
            {
                line = line.Replace(prefix, "");
            }

            return line.Trim();
        }
    }
}
