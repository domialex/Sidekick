using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Languages;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IOnAfterInit, IParserService
    {
        private readonly string[] BLOCK_SEPARATOR = new string[] { "--------" };
        private readonly string[] NEWLINE_SEPARATOR = new string[] { Environment.NewLine };

        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IStatDataService statsDataService;
        private readonly IItemDataService itemDataService;

        public ParserService(
            ILogger logger,
            ILanguageProvider languageProvider,
            IStatDataService statsDataService,
            IItemDataService itemDataService)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.statsDataService = statsDataService;
            this.itemDataService = itemDataService;
        }

        private Dictionary<Rarity, Regex> RarityPatterns { get; set; }

        public Task OnAfterInit()
        {
            RarityPatterns = new Dictionary<Rarity, Regex>();
            RarityPatterns.Add(Rarity.Normal, new Regex(Regex.Escape(languageProvider.Language.RarityNormal)));
            RarityPatterns.Add(Rarity.Magic, new Regex(Regex.Escape(languageProvider.Language.RarityMagic)));
            RarityPatterns.Add(Rarity.Rare, new Regex(Regex.Escape(languageProvider.Language.RarityRare)));
            RarityPatterns.Add(Rarity.Unique, new Regex(Regex.Escape(languageProvider.Language.RarityUnique)));
            RarityPatterns.Add(Rarity.Currency, new Regex(Regex.Escape(languageProvider.Language.RarityCurrency)));
            RarityPatterns.Add(Rarity.Gem, new Regex(Regex.Escape(languageProvider.Language.RarityGem)));
            RarityPatterns.Add(Rarity.DivinationCard, new Regex(Regex.Escape(languageProvider.Language.RarityDivinationCard)));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public async Task<Item> ParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                var item = new Item();

                var blocks = itemText
                    .Split(BLOCK_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                ParseHeader(ref item, ref blocks);

                return item;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return null;
            }
        }

        #region Item Header (Rarity, Name, Type)
        private void ParseHeader(ref Item item, ref List<string> blocks)
        {
            var headerBlock = blocks[0];

            var lines = headerBlock
                .Split(NEWLINE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            item.Rarity = GetRarity(lines[0]);

            var dataItem = itemDataService.GetItem(headerBlock);

            item.Name = dataItem.Name;
            item.TypeLine = dataItem.Type;

            blocks.Remove(headerBlock);
        }

        private Rarity GetRarity(string rarityString)
        {
            foreach (var pattern in RarityPatterns)
            {
                if (pattern.Value.IsMatch(rarityString))
                {
                    return pattern.Key;
                }
            }
            throw new Exception("Can't parse rarity.");
        }
        #endregion
    }
}
