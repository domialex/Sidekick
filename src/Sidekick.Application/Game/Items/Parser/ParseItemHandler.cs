using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Application.Game.Items.Parser.Patterns;
using Sidekick.Application.Game.Items.Parser.Tokenizers;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Modifiers;

namespace Sidekick.Application.Game.Items.Parser
{
    public class ParseItemHandler : ICommandHandler<ParseItemCommand, Item>
    {
        private readonly ILogger logger;
        private readonly IItemMetadataProvider itemMetadataProvider;
        private readonly IModifierProvider modifierProvider;
        private readonly IParserPatterns patterns;

        public ParseItemHandler(
            ILogger<ParseItemHandler> logger,
            IItemMetadataProvider itemMetadataProvider,
            IModifierProvider modifierProvider,
            IParserPatterns patterns)
        {
            this.logger = logger;
            this.itemMetadataProvider = itemMetadataProvider;
            this.modifierProvider = modifierProvider;
            this.patterns = patterns;
        }

        public Task<Item> Handle(ParseItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ItemText))
            {
                return Task.FromResult<Item>(null);
            }

            try
            {
                var itemText = new ItemNameTokenizer().CleanString(request.ItemText);
                var parsingItem = new ParsingItem(itemText);
                var metadata = itemMetadataProvider.Parse(parsingItem);

                if (metadata == null || (string.IsNullOrEmpty(metadata.Name) && string.IsNullOrEmpty(metadata.Type)))
                {
                    throw new NotSupportedException("Item not found.");
                }

                var item = new Item
                {
                    Metadata = metadata,
                    Original = new OriginalItem()
                    {
                        Name = parsingItem.NameLine,
                        Type = parsingItem.TypeLine,
                        Text = parsingItem.Text,
                    }
                };

                switch (item.Metadata.Category)
                {
                    case Category.DivinationCard:
                    case Category.Currency:
                    case Category.Prophecy:
                        break;

                    case Category.Gem:
                        ParseGem(item, parsingItem);
                        break;

                    case Category.Map:
                    case Category.Contract:
                        ParseMap(item, parsingItem);
                        ParseMods(item, parsingItem);
                        break;

                    default:
                        ParseEquipmentProperties(item, parsingItem);
                        ParseMods(item, parsingItem);
                        ParseSockets(item);
                        ParseInfluences(item, parsingItem);
                        break;
                }

                return Task.FromResult(item);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return Task.FromResult<Item>(null);
            }
        }

        private void ParseEquipmentProperties(Item item, ParsingItem parsingItem)
        {
            var propertySection = parsingItem.WholeSections[1];

            item.ItemLevel = patterns.GetInt(patterns.ItemLevel, item.Original.Text);
            item.Identified = !ParseFromEnd(patterns.Unidentified, parsingItem);
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
            item.Corrupted = ParseFromEnd(patterns.Corrupted, parsingItem);
        }

        private void ParseMap(Item item, ParsingItem parsingItem)
        {
            var mapBlock = parsingItem.MapPropertiesSection;

            item.ItemLevel = patterns.GetInt(patterns.ItemLevel, item.Original.Text);
            item.Identified = !patterns.Unidentified.IsMatch(item.Original.Text);
            item.Properties.ItemQuantity = patterns.GetInt(patterns.ItemQuantity, mapBlock);
            item.Properties.ItemRarity = patterns.GetInt(patterns.ItemRarity, mapBlock);
            item.Properties.MonsterPackSize = patterns.GetInt(patterns.MonsterPackSize, mapBlock);
            item.Properties.MapTier = patterns.GetInt(patterns.MapTier, mapBlock);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, mapBlock);
            item.Properties.Blighted = patterns.Blighted.IsMatch(parsingItem.WholeSections[0]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, parsingItem);
        }

        private void ParseSockets(Item item)
        {
            var result = patterns.Socket.Match(item.Original.Text);
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
                        groupValue = groupValue[1..];
                    }
                }
            }
        }

        private void ParseGem(Item item, ParsingItem parsingItem)
        {
            item.Properties.GemLevel = patterns.GetInt(patterns.Level, parsingItem.WholeSections[1]);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, parsingItem.WholeSections[1]);
            item.Properties.AlternateQuality = patterns.AlternateQuality.IsMatch(parsingItem.WholeSections[1]);
            item.Corrupted = ParseFromEnd(patterns.Corrupted, parsingItem);
        }

        private bool ParseFromEnd(Regex pattern, ParsingItem parsingItem)
        {
            if (parsingItem.WholeSections.Length < 3)
                return false;

            // Section order at the end:
            // Corruption
            // Influence
            // Note
            return pattern.IsMatch(parsingItem.WholeSections[^1])
                || pattern.IsMatch(parsingItem.WholeSections[^2])
                || pattern.IsMatch(parsingItem.WholeSections[^3]);
        }

        private void ParseInfluences(Item item, ParsingItem parsingItem)
        {
            item.Influences.Crusader = ParseFromEnd(patterns.Crusader, parsingItem);
            item.Influences.Elder = ParseFromEnd(patterns.Elder, parsingItem);
            item.Influences.Hunter = ParseFromEnd(patterns.Hunter, parsingItem);
            item.Influences.Redeemer = ParseFromEnd(patterns.Redeemer, parsingItem);
            item.Influences.Shaper = ParseFromEnd(patterns.Shaper, parsingItem);
            item.Influences.Warlord = ParseFromEnd(patterns.Warlord, parsingItem);
        }

        private void ParseMods(Item item, ParsingItem parsingItem)
        {
            item.Modifiers = modifierProvider.Parse(parsingItem);
        }
    }
}
