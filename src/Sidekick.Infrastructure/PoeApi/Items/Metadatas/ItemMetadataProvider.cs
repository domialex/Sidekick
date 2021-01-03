using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Application.Game.Items.Parser.Extensions;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Metadatas.Models;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Infrastructure.PoeApi.Items.Metadatas.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Metadatas
{
    public class ItemMetadataProvider : IItemMetadataProvider
    {
        private readonly ICacheRepository cacheRepository;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly IPoeTradeClient poeTradeClient;

        public ItemMetadataProvider(ICacheRepository cacheRepository,
            IGameLanguageProvider gameLanguageProvider,
            IPoeTradeClient poeTradeClient)
        {
            this.cacheRepository = cacheRepository;
            this.gameLanguageProvider = gameLanguageProvider;
            this.poeTradeClient = poeTradeClient;
        }

        private Dictionary<string, List<ItemMetadata>> NameAndTypeDictionary { get; set; }
        private List<(Regex Regex, ItemMetadata Item)> NameAndTypeRegex { get; set; }
        private string[] Prefixes { get; set; }

        public async Task Initialize()
        {
            NameAndTypeDictionary = new Dictionary<string, List<ItemMetadata>>();
            NameAndTypeRegex = new List<(Regex Regex, ItemMetadata Item)>();

            var result = await cacheRepository.GetOrSet(
                "Sidekick.Infrastructure.PoeApi.Items.Metadatas.ItemMetadataProvider.Initialize",
                () => poeTradeClient.Fetch<ApiCategory>("data/items"));

            FillPattern(result.Result[0].Entries, Category.Accessory, useRegex: true);
            FillPattern(result.Result[1].Entries, Category.Armour, useRegex: true);
            FillPattern(result.Result[2].Entries, Category.DivinationCard);
            FillPattern(result.Result[3].Entries, Category.Currency);
            FillPattern(result.Result[4].Entries, Category.Flask, useRegex: true);
            FillPattern(result.Result[5].Entries, Category.Gem);
            FillPattern(result.Result[6].Entries, Category.Jewel, useRegex: true);
            FillPattern(result.Result[7].Entries, Category.Map, useRegex: true);
            FillPattern(result.Result[8].Entries, Category.Weapon, useRegex: true);
            FillPattern(result.Result[9].Entries, Category.Leaguestone);
            FillPattern(result.Result[10].Entries, Category.Prophecy);
            FillPattern(result.Result[11].Entries, Category.ItemisedMonster, useRegex: true);
            FillPattern(result.Result[12].Entries, Category.Watchstone);
            FillPattern(result.Result[14].Entries, Category.Contract, useRegex: true);

            Prefixes = new[]
            {
                gameLanguageProvider.Language.PrefixSuperior,
                gameLanguageProvider.Language.PrefixBlighted,
                gameLanguageProvider.Language.PrefixAnomalous,
                gameLanguageProvider.Language.PrefixDivergent,
                gameLanguageProvider.Language.PrefixPhantasmal,
            };
        }

        private void FillPattern(List<ApiItem> items, Category category, bool useRegex = false)
        {
            foreach (var item in items)
            {
                var header = new ItemMetadata()
                {
                    Name = item.Name,
                    Type = item.Type,
                    Rarity = GetRarityForCategory(category, item),
                    Category = category,
                };

                var key = item.Name ?? item.Type;

                if (useRegex)
                {
                    NameAndTypeRegex.Add((key.ToRegex(), header));
                }

                if (!NameAndTypeDictionary.TryGetValue(key, out var dictionaryEntry))
                {
                    dictionaryEntry = new List<ItemMetadata>();
                    NameAndTypeDictionary.Add(key, dictionaryEntry);
                }

                dictionaryEntry.Add(header);
            }
        }

        private Rarity GetRarityForCategory(Category category, ApiItem item)
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

        public IItemMetadata Parse(ParsingItem parsingItem, Rarity itemRarity)
        {
            var results = new List<ItemMetadata>();

            // Rares may have conflicting names, so we don't want to search any unique items that may have that name. Like "Ancient Orb" which can be used by abyss jewels.
            // There are some items which have prefixes which we don't want to remove, like the "Blighted Delirium Orb".
            if (itemRarity != Rarity.Rare && NameAndTypeDictionary.TryGetValue(parsingItem.HeaderSection[1], out var itemData))
            {
                results.AddRange(itemData);
            }
            else if (itemRarity != Rarity.Rare && NameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(parsingItem.HeaderSection[1]), out itemData))
            {
                results.AddRange(itemData);
            }
            else if (parsingItem.HeaderSection.Length > 2 && NameAndTypeDictionary.TryGetValue(GetLineWithoutPrefixes(parsingItem.HeaderSection[2]), out itemData))
            {
                results.AddRange(itemData);
            }
            else
            {
                results.AddRange(NameAndTypeRegex
                    .Where(pattern => pattern.Regex.IsMatch(parsingItem.WholeSections[0]))
                    .Select(x => x.Item));
            }

            if (results.Any(item => item.Rarity == Rarity.Gem)
                && parsingItem.TryGetVaalGemName(out var vaalGemName)
                && NameAndTypeDictionary.TryGetValue(vaalGemName, out itemData))
            {
                // If we find a Vaal gem, we don't care about other matches
                results.Clear();
                results.Add(itemData.First());
            }

            return results
                .OrderBy(x => x.Rarity == Rarity.Unique ? 0 : 1)
                .ThenBy(x => x.Rarity == Rarity.Unknown ? 0 : 1)
                .ThenBy(x => x.Type == parsingItem.HeaderSection.Last() ? 0 : 1)
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
