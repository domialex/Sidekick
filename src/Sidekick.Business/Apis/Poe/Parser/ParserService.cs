using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser.ItemParsers;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Languages;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IOnAfterInit, IParserService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IStatDataService statsDataService;
        private readonly IItemDataService itemDataService;
        private readonly IParserPatterns patterns;
        private readonly ItemNameTokenizer itemNameTokenizer;
        public ParserService(
            ILogger logger,
            ILanguageProvider languageProvider,
            IStatDataService statsDataService,
            IItemDataService itemDataService,
            IParserPatterns patterns)
        {
            this.logger = logger.ForContext(GetType());
            this.languageProvider = languageProvider;
            this.statsDataService = statsDataService;
            this.itemDataService = itemDataService;
            this.patterns = patterns;
            itemNameTokenizer = new ItemNameTokenizer();
        }

        private const string NEWLINE_PATTERN = "\r\n";
        private const string SEPARATOR_PATTERN = "--------";

        public Task OnAfterInit()
        {
            // Not needed?
            return Task.CompletedTask;
        }

        public async Task<ParsedItem> ParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                itemText = itemNameTokenizer.CleanString(itemText);

                var item = new ParsedItem
                {
                    ItemText = itemText
                };

                var textBlocks = new ItemTextBlock(itemText
                    .Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries)
                    .Select(block => block.Split(NEWLINE_PATTERN, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray());

                var dataItem = itemDataService.ParseItemData(textBlocks);

                item.Name = dataItem.Name;
                item.TypeLine = dataItem.Type;
                item.Rarity = dataItem.Rarity;

                if (item.Rarity == Rarity.Unknown)
                {
                    item.Rarity = GetRarity(textBlocks.Header[0]);
                }

                ParseHeader(ref item, ref itemText);
                ParseProperties(ref item, ref itemText);
                ParseSockets(ref item, ref itemText);
                ParseInfluences(ref item, ref itemText);
                ParseMods(ref item, ref itemText);

                return item;
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not parse item.");
                return null;
            }
        }

        public async Task<ParsedItem> NewParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                itemText = itemNameTokenizer.CleanString(itemText);

                var textBlocks = new ItemTextBlock(itemText
                    .Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries)
                    .Select(block => block.Split(NEWLINE_PATTERN, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray());

                var itemData = itemDataService.ParseItemData(textBlocks);

                if (itemData.Rarity == Rarity.Unknown)
                {
                    itemData.Rarity = GetRarity(textBlocks.Header[0]);
                }

                //var parser = GetParser(textBlocks, itemData);

                //return parser.ParseItem();
                return null;
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not parse item.");
                return null;
            }
        }

        //private ItemParser GetParser(ItemTextBlock itemText, ItemData itemData)
        //{

        //}

        private void ParseHeader(ref ParsedItem item, ref string input)
        {
            if (string.IsNullOrEmpty(item.Name) && string.IsNullOrEmpty(item.TypeLine))
            {
                throw new NotSupportedException("Item not found.");
            }

            if (item.Rarity != Rarity.DivinationCard)
            {
                item.ItemLevel = GetInt(patterns.ItemLevel, input);
                item.Identified = !patterns.Unidentified.IsMatch(input);
                item.Corrupted = patterns.Corrupted.IsMatch(input);
            }
        }

        private Rarity GetRarity(string rarityString)
        {
            foreach (var pattern in patterns.Rarity)
            {
                if (pattern.Value.IsMatch(rarityString))
                {
                    return pattern.Key;
                }
            }
            throw new Exception("Can't parse rarity.");
        }

        private void ParseProperties(ref ParsedItem item, ref string input)
        {
            var blocks = input.Split(SEPARATOR_PATTERN);

            item.Armor = GetInt(patterns.Armor, blocks[1]);
            item.EnergyShield = GetInt(patterns.EnergyShield, blocks[1]);
            item.Evasion = GetInt(patterns.Evasion, blocks[1]);
            item.ChanceToBlock = GetInt(patterns.ChanceToBlock, blocks[1]);
            item.Quality = GetInt(patterns.Quality, blocks[1]);
            item.MapTier = GetInt(patterns.MapTier, blocks[1]);
            item.ItemQuantity = GetInt(patterns.ItemQuantity, blocks[1]);
            item.ItemRarity = GetInt(patterns.ItemRarity, blocks[1]);
            item.MonsterPackSize = GetInt(patterns.MonsterPackSize, blocks[1]);
            item.AttacksPerSecond = GetDouble(patterns.AttacksPerSecond, blocks[1]);
            item.CriticalStrikeChance = GetDouble(patterns.CriticalStrikeChance, blocks[1]);
            item.Extended.ElementalDps = GetDps(patterns.ElementalDamage, blocks[1], item.AttacksPerSecond);
            item.Extended.PhysicalDps = GetDps(patterns.PhysicalDamage, blocks[1], item.AttacksPerSecond);
            item.Extended.DamagePerSecond = item.Extended.ElementalDps + item.Extended.PhysicalDps;
            item.Blighted = patterns.Blighted.IsMatch(blocks[0]);

            if (item.Rarity == Rarity.Gem)
            {
                item.GemLevel = GetInt(patterns.Level, blocks[1]);
            }
        }

        private void ParseSockets(ref ParsedItem item, ref string input)
        {
            var result = patterns.Socket.Match(input);
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

        private void ParseInfluences(ref ParsedItem item, ref string input)
        {
            var blocks = input.Split(SEPARATOR_PATTERN);
            var strippedInput = string.Concat(blocks.Skip(1).ToList());

            item.Influences.Crusader = patterns.Crusader.IsMatch(strippedInput);
            item.Influences.Elder = patterns.Elder.IsMatch(strippedInput);
            item.Influences.Hunter = patterns.Hunter.IsMatch(strippedInput);
            item.Influences.Redeemer = patterns.Redeemer.IsMatch(strippedInput);
            item.Influences.Shaper = patterns.Shaper.IsMatch(strippedInput);
            item.Influences.Warlord = patterns.Warlord.IsMatch(strippedInput);
        }

        private void ParseMods(ref ParsedItem item, ref string input)
        {
            item.Extended.Mods = statsDataService.ParseMods(input);
        }

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

        private double GetDouble(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    if (double.TryParse(match.Groups[1].Value.Replace(",", "."), out var result))
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        private double GetDps(Regex regex, string input, double attacksPerSecond)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    var split = match.Groups[1].Value.Split('-');

                    if (int.TryParse(split[0], out var minValue) && int.TryParse(split[1], out var maxValue))
                    {
                        return ((minValue + maxValue) / 2) * attacksPerSecond;
                    }
                }
            }

            return 0;
        }
        #endregion
    }
}
