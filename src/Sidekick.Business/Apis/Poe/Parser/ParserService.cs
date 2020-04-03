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

                var wholeSections = itemText.Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries);
                var splitSections = wholeSections
                    .Select(block => block.Split(NEWLINE_PATTERN, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray();

                var itemSections = new ItemSections(splitSections, wholeSections);

                var itemData = itemDataService.ParseItemData(itemSections);

                if (itemData.Rarity == Rarity.Unknown)
                {
                    itemData.Rarity = GetRarity(itemSections.Rarity);
                }

                if (string.IsNullOrEmpty(itemData.Name) && string.IsNullOrEmpty(itemData.Type))
                {
                    throw new NotSupportedException("Item not found.");
                }

                return ParseItemDetails(itemText, itemSections, itemData);
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not parse item.");
                return null;
            }
        }

        private ParsedItem ParseItemDetails(string itemText, ItemSections itemSections, ItemData itemData)
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
                    ParseGem(item, itemSections);
                    break;

                case var _ when ItemIsMap(itemSections):
                    ParseMap(item, itemSections);
                    ParseMods(item);
                    break;

                default:
                    ParseEquipmentProperties(item, itemSections);
                    ParseMods(item);
                    ParseSockets(item);
                    ParseInfluences(item, itemSections);
                    break;
            }

            return item;
        }

        private bool ItemIsMap(ItemSections itemTextBlock)
        {
            return itemTextBlock.TryGetMapTierLine(out var mapTierLine) && patterns.MapTier.IsMatch(mapTierLine);
        }

        private void ParseEquipmentProperties(ParsedItem item, ItemSections itemSections)
        {
            var propertySection = itemSections.WholeSections[1];

            item.ItemLevel = GetInt(patterns.ItemLevel, item.ItemText);
            item.Identified = !ParseFromEnd(patterns.Unidentified, itemSections);
            item.Armor = GetInt(patterns.Armor, propertySection);
            item.EnergyShield = GetInt(patterns.EnergyShield, propertySection);
            item.Evasion = GetInt(patterns.Evasion, propertySection);
            item.ChanceToBlock = GetInt(patterns.ChanceToBlock, propertySection);
            item.Quality = GetInt(patterns.Quality, propertySection);
            item.AttacksPerSecond = GetDouble(patterns.AttacksPerSecond, propertySection);
            item.CriticalStrikeChance = GetDouble(patterns.CriticalStrikeChance, propertySection);
            item.Extended.ElementalDps = GetDps(patterns.ElementalDamage, propertySection, item.AttacksPerSecond);
            item.Extended.PhysicalDps = GetDps(patterns.PhysicalDamage, propertySection, item.AttacksPerSecond);
            item.Extended.DamagePerSecond = item.Extended.ElementalDps + item.Extended.PhysicalDps;
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);
        }

        private void ParseMap(ParsedItem item, ItemSections itemSections)
        {
            var mapBlock = itemSections.MapPropertiesSection;

            item.ItemLevel = GetInt(patterns.ItemLevel, item.ItemText);
            item.Identified = !patterns.Unidentified.IsMatch(item.ItemText);
            item.ItemQuantity = GetInt(patterns.ItemQuantity, mapBlock);
            item.ItemRarity = GetInt(patterns.ItemRarity, mapBlock);
            item.MonsterPackSize = GetInt(patterns.MonsterPackSize, mapBlock);
            item.MapTier = GetInt(patterns.MapTier, mapBlock);
            item.Blighted = patterns.Blighted.IsMatch(itemSections.WholeSections[0]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);

            // Needs to be implemented in query, I think
            //item.Influences.Shaper = patterns.Shaper.IsMatch(itemSections.MapInfluenceSection);
            //item.Influences.Elder = patterns.Elder.IsMatch(itemSections.MapInfluenceSection);
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

        private void ParseGem(ParsedItem item, ItemSections itemSections)
        {
            item.GemLevel = GetInt(patterns.Level, itemSections.WholeSections[1]);
            item.Quality = GetInt(patterns.Quality, itemSections.WholeSections[1]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);
        }

        private bool ParseFromEnd(Regex pattern, ItemSections itemSections)
        {
            // Section order at the end:
            // Corruption
            // Influence
            // Note
            return pattern.IsMatch(itemSections.WholeSections[^1])
                || pattern.IsMatch(itemSections.WholeSections[^2])
                || pattern.IsMatch(itemSections.WholeSections[^3]);
        }

        private void ParseInfluences(ParsedItem item, ItemSections itemSections)
        {
            item.Influences.Crusader = ParseFromEnd(patterns.Crusader, itemSections);
            item.Influences.Elder = ParseFromEnd(patterns.Elder, itemSections);
            item.Influences.Hunter = ParseFromEnd(patterns.Hunter, itemSections);
            item.Influences.Redeemer = ParseFromEnd(patterns.Redeemer, itemSections);
            item.Influences.Shaper = ParseFromEnd(patterns.Shaper, itemSections);
            item.Influences.Warlord = ParseFromEnd(patterns.Warlord, itemSections);
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
