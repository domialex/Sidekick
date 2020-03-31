using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
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

                var wholeBlocks = itemText.Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries);
                var splitBlocks = wholeBlocks.Select(block => block.Split(NEWLINE_PATTERN, StringSplitOptions.RemoveEmptyEntries));

                var itemTextBlock = new ItemTextBlock(splitBlocks.ToArray(), wholeBlocks);

                var itemData = itemDataService.ParseItemData(itemTextBlock);

                if (itemData.Rarity == Rarity.Unknown)
                {
                    itemData.Rarity = GetRarity(itemTextBlock.Rarity);
                }

                if (string.IsNullOrEmpty(itemData.Name) && string.IsNullOrEmpty(itemData.Type))
                {
                    throw new NotSupportedException("Item not found.");
                }

                return ParseItemDetails(itemText, itemTextBlock, itemData);
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not parse item.");
                return null;
            }
        }

        private ParsedItem ParseItemDetails(string itemText, ItemTextBlock itemTextBlock, ItemData itemData)
        {
            var item = new ParsedItem
            {
                ItemText = itemText,
                Name = itemData.Name,
                TypeLine = itemData.Type,
                Rarity = itemData.Rarity
            };

            switch (item.Rarity)
            {
                case Rarity.DivinationCard:
                case Rarity.Currency:
                case Rarity.Prophecy:
                    break;

                case Rarity.Gem:
                    ParseGem(item, itemTextBlock);
                    break;

                case var _ when ItemIsMap(itemTextBlock):
                    ParseMap(item, itemTextBlock);
                    ParseInfluences(item, itemTextBlock);
                    break;

                default:
                    ParseEquipmentProperties(item, itemTextBlock);
                    ParseMods(item);
                    ParseSockets(item);
                    ParseInfluences(item, itemTextBlock);
                    break;
            }

            if (item.Rarity != Rarity.DivinationCard)
            {
                item.ItemLevel = GetInt(patterns.ItemLevel, item.ItemText);
                item.Identified = !patterns.Unidentified.IsMatch(item.ItemText);
                item.Corrupted = patterns.Corrupted.IsMatch(item.ItemText);
            }

            return item;
        }

        private bool ItemIsMap(ItemTextBlock itemTextBlock)
        {
            return itemTextBlock.TryGetMapTierLine(out var mapTierLine) && patterns.MapTier.IsMatch(mapTierLine);
        }

        private void ParseEquipmentProperties(ParsedItem item, ItemTextBlock itemTextBlock)
        {
            var propertyBlock = itemTextBlock.WholeBlocks[1];

            item.Armor = GetInt(patterns.Armor, propertyBlock);
            item.EnergyShield = GetInt(patterns.EnergyShield, propertyBlock);
            item.Evasion = GetInt(patterns.Evasion, propertyBlock);
            item.ChanceToBlock = GetInt(patterns.ChanceToBlock, propertyBlock);
            item.Quality = GetInt(patterns.Quality, propertyBlock);
            item.AttacksPerSecond = GetDouble(patterns.AttacksPerSecond, propertyBlock);
            item.CriticalStrikeChance = GetDouble(patterns.CriticalStrikeChance, propertyBlock);
            item.Extended.ElementalDps = GetDps(patterns.ElementalDamage, propertyBlock, item.AttacksPerSecond);
            item.Extended.PhysicalDps = GetDps(patterns.PhysicalDamage, propertyBlock, item.AttacksPerSecond);
            item.Extended.DamagePerSecond = item.Extended.ElementalDps + item.Extended.PhysicalDps;
        }

        private void ParseMap(ParsedItem item, ItemTextBlock itemTextBlock)
        {
            var mapBlock = itemTextBlock.MapPropertiesBlock;

            item.ItemQuantity = GetInt(patterns.ItemQuantity, mapBlock);
            item.ItemRarity = GetInt(patterns.ItemRarity, mapBlock);
            item.MonsterPackSize = GetInt(patterns.MonsterPackSize, mapBlock);
            item.MapTier = GetInt(patterns.MapTier, mapBlock);
            item.Blighted = patterns.Blighted.IsMatch(itemTextBlock.WholeBlocks[0]);
        }

        private void ParseSockets(ParsedItem item)
        {
            var result = patterns.Socket.Match(item.ItemText);
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

        private void ParseGem(ParsedItem item, ItemTextBlock itemTextBlock)
        {
            item.GemLevel = GetInt(patterns.Level, itemTextBlock.WholeBlocks[1]);
            item.Quality = GetInt(patterns.Quality, itemTextBlock.WholeBlocks[1]);
        }

        private void ParseInfluences(ParsedItem item, ItemTextBlock itemTextBlock)
        {
            var block = string.Concat(itemTextBlock.WholeBlocks.Skip(1));

            item.Influences.Crusader = patterns.Crusader.IsMatch(block);
            item.Influences.Elder = patterns.Elder.IsMatch(block);
            item.Influences.Hunter = patterns.Hunter.IsMatch(block);
            item.Influences.Redeemer = patterns.Redeemer.IsMatch(block);
            item.Influences.Shaper = patterns.Shaper.IsMatch(block);
            item.Influences.Warlord = patterns.Warlord.IsMatch(block);
        }

        private void ParseMods(ParsedItem item)
        {
            item.Extended.Mods = statsDataService.ParseMods(item.ItemText);
        }

        #region Helpers

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
