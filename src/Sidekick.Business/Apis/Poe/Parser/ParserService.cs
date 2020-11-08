using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IParserService
    {
        private readonly ILogger logger;
        private readonly IStatDataService statsDataService;
        private readonly IItemDataService itemDataService;
        private readonly IParserPatterns patterns;
        private readonly ItemNameTokenizer itemNameTokenizer;

        private readonly Regex newLinePattern = new Regex("[\\r\\n]+");

        private const string SEPARATOR_PATTERN = "--------";

        public ParserService(
            ILogger<ParserService> logger,
            IStatDataService statsDataService,
            IItemDataService itemDataService,
            IParserPatterns patterns)
        {
            this.logger = logger;
            this.statsDataService = statsDataService;
            this.itemDataService = itemDataService;
            this.patterns = patterns;
            itemNameTokenizer = new ItemNameTokenizer();
        }

        public Item ParseItem(string itemText)
        {
            if (string.IsNullOrEmpty(itemText))
            {
                return null;
            }

            try
            {
                itemText = itemNameTokenizer.CleanString(itemText);

                var wholeSections = itemText.Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries);
                var splitSections = wholeSections
                    .Select(block => newLinePattern
                        .Split(block)
                        .Where(line => line != "")
                        .ToArray())
                    .ToArray();

                var itemSections = new ItemSections(splitSections, wholeSections);

                var itemData = itemDataService.ParseItemData(itemSections, GetRarity(itemSections.Rarity));

                if (itemData == null || string.IsNullOrEmpty(itemData.Name) && string.IsNullOrEmpty(itemData.Type))
                {
                    throw new NotSupportedException("Item not found.");
                }

                if (itemData.Rarity == Rarity.Unknown)
                {
                    itemData.Rarity = GetRarity(itemSections.Rarity);
                }

                return ParseItemDetails(itemText, itemSections, itemData);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return null;
            }
        }

        private Item ParseItemDetails(string itemText, ItemSections itemSections, ItemData itemData)
        {
            var item = new Item
            {
                Text = itemText,
                Name = itemData.Name,
                Type = itemData.Type,
                NameLine = itemSections.NameLine,
                TypeLine = itemSections.TypeLine,
                Rarity = itemData.Rarity,
                Category = itemData.Category,
            };

            switch (itemData.Category)
            {
                case Category.DivinationCard:
                case Category.Currency:
                case Category.Prophecy:
                    break;

                case Category.Gem:
                    ParseGem(item, itemSections);
                    break;

                case Category.Map:
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

        private void ParseEquipmentProperties(Item item, ItemSections itemSections)
        {
            var propertySection = itemSections.WholeSections[1];

            item.ItemLevel = patterns.GetInt(patterns.ItemLevel, item.Text);
            item.Identified = !ParseFromEnd(patterns.Unidentified, itemSections);
            item.Properties.Armor = patterns.GetInt(patterns.Armor, propertySection);
            item.Properties.EnergyShield = patterns.GetInt(patterns.EnergyShield, propertySection);
            item.Properties.Evasion = patterns.GetInt(patterns.Evasion, propertySection);
            item.Properties.ChanceToBlock = patterns.GetInt(patterns.ChanceToBlock, propertySection);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, propertySection);
            item.Properties.AttacksPerSecond = patterns.GetDouble(patterns.AttacksPerSecond, propertySection);
            item.Properties.CriticalStrikeChance = patterns.GetDouble(patterns.CriticalStrikeChance, propertySection);
            item.Properties.ElementalDps = patterns.GetDps(patterns.ElementalDamage, propertySection, item.Properties.AttacksPerSecond);
            item.Properties.PhysicalDps = patterns.GetDps(patterns.PhysicalDamage, propertySection, item.Properties.AttacksPerSecond);
            item.Properties.DamagePerSecond = item.Properties.ElementalDps + item.Properties.PhysicalDps;
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);
        }

        private void ParseMap(Item item, ItemSections itemSections)
        {
            var mapBlock = itemSections.MapPropertiesSection;

            item.ItemLevel = patterns.GetInt(patterns.ItemLevel, item.Text);
            item.Identified = !patterns.Unidentified.IsMatch(item.Text);
            item.Properties.ItemQuantity = patterns.GetInt(patterns.ItemQuantity, mapBlock);
            item.Properties.ItemRarity = patterns.GetInt(patterns.ItemRarity, mapBlock);
            item.Properties.MonsterPackSize = patterns.GetInt(patterns.MonsterPackSize, mapBlock);
            item.Properties.MapTier = patterns.GetInt(patterns.MapTier, mapBlock);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, mapBlock);
            item.Properties.Blighted = patterns.Blighted.IsMatch(itemSections.WholeSections[0]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);
        }

        private void ParseSockets(Item item)
        {
            var result = patterns.Socket.Match(item.Text);
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
                            case 'B': item.Sockets.Add(new Socket() { Group = index - 1, Colour = SocketColour.Blue }); break;
                            case 'G': item.Sockets.Add(new Socket() { Group = index - 1, Colour = SocketColour.Green }); break;
                            case 'R': item.Sockets.Add(new Socket() { Group = index - 1, Colour = SocketColour.Red }); break;
                            case 'W': item.Sockets.Add(new Socket() { Group = index - 1, Colour = SocketColour.White }); break;
                            case 'A': item.Sockets.Add(new Socket() { Group = index - 1, Colour = SocketColour.Abyss }); break;
                        }
                        groupValue = groupValue.Substring(1);
                    }
                }
            }
        }

        private void ParseGem(Item item, ItemSections itemSections)
        {
            item.Properties.GemLevel = patterns.GetInt(patterns.Level, itemSections.WholeSections[1]);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, itemSections.WholeSections[1]);
            item.Properties.AlternateQuality = patterns.AlternateQuality.IsMatch(itemSections.WholeSections[1]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, itemSections);
        }

        private bool ParseFromEnd(Regex pattern, ItemSections itemSections)
        {
            if (itemSections.WholeSections.Length < 3)
                return false;

            // Section order at the end:
            // Corruption
            // Influence
            // Note
            return pattern.IsMatch(itemSections.WholeSections[^1])
                || pattern.IsMatch(itemSections.WholeSections[^2])
                || pattern.IsMatch(itemSections.WholeSections[^3]);
        }

        private void ParseInfluences(Item item, ItemSections itemSections)
        {
            item.Influences.Crusader = ParseFromEnd(patterns.Crusader, itemSections);
            item.Influences.Elder = ParseFromEnd(patterns.Elder, itemSections);
            item.Influences.Hunter = ParseFromEnd(patterns.Hunter, itemSections);
            item.Influences.Redeemer = ParseFromEnd(patterns.Redeemer, itemSections);
            item.Influences.Shaper = ParseFromEnd(patterns.Shaper, itemSections);
            item.Influences.Warlord = ParseFromEnd(patterns.Warlord, itemSections);
        }

        private void ParseMods(Item item)
        {
            item.Modifiers = statsDataService.ParseMods(item.Text);
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
    }
}
