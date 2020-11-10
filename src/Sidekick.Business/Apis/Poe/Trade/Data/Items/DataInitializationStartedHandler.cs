using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IItemDataService itemDataService;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly ILanguageProvider languageProvider;
        private readonly ICacheRepository cacheRepository;

        public DataInitializationStartedHandler(
            IItemDataService itemDataService,
            IPoeTradeClient poeTradeClient,
            ILanguageProvider languageProvider,
            ICacheRepository cacheRepository)
        {
            this.itemDataService = itemDataService;
            this.poeTradeClient = poeTradeClient;
            this.languageProvider = languageProvider;
            this.cacheRepository = cacheRepository;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            itemDataService.NameAndTypeDictionary = new Dictionary<string, List<ItemData>>();
            itemDataService.NameAndTypeRegex = new List<(Regex Regex, ItemData Item)>();

            var categories = await cacheRepository.GetOrSet(
                "Sidekick.Business.Apis.Poe.Trade.Data.Items.InitializeDataHandler",
                () => poeTradeClient.Fetch<ItemDataCategory>());

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

            itemDataService.Prefixes = new[]
            {
                languageProvider.Language.PrefixSuperior,
                languageProvider.Language.PrefixBlighted,
                languageProvider.Language.PrefixAnomalous,
                languageProvider.Language.PrefixDivergent,
                languageProvider.Language.PrefixPhantasmal,
            };
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
                    itemDataService.NameAndTypeRegex.Add((key.ToRegex(), item));
                }

                if (!itemDataService.NameAndTypeDictionary.TryGetValue(key, out var itemData))
                {
                    itemData = new List<ItemData>();
                    itemDataService.NameAndTypeDictionary.Add(key, itemData);
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
    }
}
