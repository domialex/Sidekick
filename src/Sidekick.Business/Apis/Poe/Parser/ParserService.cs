using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Languages;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IOnAfterInit, IParserService
    {
        private readonly string[] BLOCK_SEPARATOR = new string[] { "--------" };
        private readonly string[] NEWLINE_SEPARATOR = new string[] { "\r", "\n" };

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

        public Task OnAfterInit()
        {
            InitHeader();
            InitProperties();
            InitSockets();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public async Task<ParsedItem> ParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                var item = new ParsedItem();

                var blocks = itemText
                    .Split(BLOCK_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                ParseHeader(ref item, ref blocks);
                ParseProperties(ref item, ref blocks);
                ParseSockets(ref item, ref itemText);

                return item;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return null;
            }
        }

        #region Header (Rarity, Name, Type)
        private Dictionary<Rarity, Regex> RarityPatterns { get; set; }

        private void InitHeader()
        {
            RarityPatterns = new Dictionary<Rarity, Regex>();
            RarityPatterns.Add(Rarity.Normal, new Regex(Regex.Escape(languageProvider.Language.RarityNormal)));
            RarityPatterns.Add(Rarity.Magic, new Regex(Regex.Escape(languageProvider.Language.RarityMagic)));
            RarityPatterns.Add(Rarity.Rare, new Regex(Regex.Escape(languageProvider.Language.RarityRare)));
            RarityPatterns.Add(Rarity.Unique, new Regex(Regex.Escape(languageProvider.Language.RarityUnique)));
            RarityPatterns.Add(Rarity.Currency, new Regex(Regex.Escape(languageProvider.Language.RarityCurrency)));
            RarityPatterns.Add(Rarity.Gem, new Regex(Regex.Escape(languageProvider.Language.RarityGem)));
            RarityPatterns.Add(Rarity.DivinationCard, new Regex(Regex.Escape(languageProvider.Language.RarityDivinationCard)));
        }

        private void ParseHeader(ref ParsedItem item, ref List<string> blocks)
        {
            var headerBlock = new ItemNameTokenizer().CleanString(blocks[0]);

            var lines = headerBlock
                .Split(NEWLINE_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            item.Rarity = GetRarity(lines[0]);

            var dataItem = itemDataService.GetItem(headerBlock);

            item.Name = dataItem.Name;
            item.TypeLine = dataItem.Type;

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.Name = lines[1];
            }

            blocks.RemoveAt(0);
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

        #region Properties (Armour, Evasion, Energy Shield, Quality)
        private Regex ArmorPattern { get; set; }
        private Regex EnergyShieldPattern { get; set; }
        private Regex EvasionPattern { get; set; }
        private Regex QualityPattern { get; set; }

        private void InitProperties()
        {
            ArmorPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionArmour)}[^\\r\\n\\d]*(\\d+)");
            EnergyShieldPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionEnergyShield)}[^\\r\\n\\d]*(\\d+)");
            EvasionPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionEvasion)}[^\\r\\n\\d]*(\\d+)");
            QualityPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionQuality)}[^\\r\\n\\d]*(\\d+)");
        }

        private void ParseProperties(ref ParsedItem item, ref List<string> blocks)
        {
            var block = blocks[0];

            item.Armor = GetInt(ArmorPattern, block);
            item.EnergyShield = GetInt(EnergyShieldPattern, block);
            item.Evasion = GetInt(EvasionPattern, block);
            item.Quality = GetInt(QualityPattern, block);

            if (item.Armor + item.EnergyShield + item.Evasion > 0)
            {
                blocks.RemoveAt(0);
            }
        }
        #endregion

        #region Sockets
        private Regex SocketPattern { get; set; }

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            SocketPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionSockets)}[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }

        private void ParseSockets(ref ParsedItem item, ref string input)
        {
            var result = SocketPattern.Match(input);
            if (result.Success)
            {
                var groups = result.Groups
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(x => x.Value)
                    .ToList();
                for (var index = 1; index < groups.Count; index++)
                {
                    var groupValue = groups[index].Replace("-", "").Trim();
                    while (groupValue.Length > 0)
                    {
                        switch (groupValue[0])
                        {
                            case 'B': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Blue }); break;
                            case 'G': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Green }); break;
                            case 'R': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Red }); break;
                            case 'W': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.White }); break;
                            case 'A': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Abyss }); break;
                        }
                        groupValue = groupValue.Substring(1);
                    }
                }
            }
        }
        #endregion

        #region Helpers
        private int GetInt(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    if (int.TryParse(match.Groups[1].Value, out var result))
                    {
                        return result;
                    }
                }
            }

            return 0;
        }
        #endregion
    }
}
