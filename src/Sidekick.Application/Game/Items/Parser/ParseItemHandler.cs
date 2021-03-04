using System;
using System.Collections.Generic;
using System.Globalization;
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
using Sidekick.Domain.Game.Modifiers.Models;

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
                parsingItem.Metadata = itemMetadataProvider.Parse(parsingItem);

                if (parsingItem.Metadata == null || (string.IsNullOrEmpty(parsingItem.Metadata.Name) && string.IsNullOrEmpty(parsingItem.Metadata.Type)))
                {
                    throw new NotSupportedException("Item not found.");
                }

                var item = new Item
                {
                    Metadata = parsingItem.Metadata,
                    Original = ParseOriginal(parsingItem),
                    Properties = ParseProperties(parsingItem),
                    Influences = ParseInfluences(parsingItem),
                    Sockets = ParseSockets(parsingItem),
                    Modifiers = ParseModifiers(parsingItem),
                };

                return Task.FromResult(item);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return Task.FromResult<Item>(null);
            }
        }

        private OriginalItem ParseOriginal(ParsingItem parsingItem)
        {
            return new OriginalItem()
            {
                Name = parsingItem.Blocks[0].Lines.ElementAtOrDefault(1)?.Text,
                Type = parsingItem.Blocks[0].Lines.ElementAtOrDefault(2)?.Text,
                Text = parsingItem.Text,
            };
        }

        private Properties ParseProperties(ParsingItem parsingItem)
        {
            switch (parsingItem.Metadata.Category)
            {
                case Category.Gem:
                    return ParseGemProperties(parsingItem);

                case Category.Map:
                case Category.Contract:
                    return ParseMapProperties(parsingItem);

                case Category.Accessory:
                    return ParseAccessoryProperties(parsingItem);

                case Category.Armour:
                    return ParseArmourProperties(parsingItem);

                case Category.Weapon:
                    return ParseWeaponProperties(parsingItem);

                case Category.Jewel:
                    return ParseJewelProperties(parsingItem);

                default:
                    return null;
            }
        }

        private Properties ParseWeaponProperties(ParsingItem parsingItem)
        {
            var propertyBlock = parsingItem.Blocks[1];

            var properties = new Properties
            {
                ItemLevel = GetInt(patterns.ItemLevel, parsingItem),
                Identified = !GetBool(patterns.Unidentified, parsingItem),
                Corrupted = GetBool(patterns.Corrupted, parsingItem),

                Quality = GetInt(patterns.Quality, propertyBlock),
                AttacksPerSecond = GetDouble(patterns.AttacksPerSecond, propertyBlock),
                CriticalStrikeChance = GetDouble(patterns.CriticalStrikeChance, propertyBlock)
            };

            properties.ElementalDps = GetDps(patterns.ElementalDamage, propertyBlock, properties.AttacksPerSecond);
            properties.PhysicalDps = GetDps(patterns.PhysicalDamage, propertyBlock, properties.AttacksPerSecond);
            properties.DamagePerSecond = properties.ElementalDps + properties.PhysicalDps;

            return properties;
        }

        private Properties ParseArmourProperties(ParsingItem parsingItem)
        {
            var propertyBlock = parsingItem.Blocks[1];

            return new Properties()
            {
                ItemLevel = GetInt(patterns.ItemLevel, parsingItem),
                Identified = !GetBool(patterns.Unidentified, parsingItem),
                Corrupted = GetBool(patterns.Corrupted, parsingItem),

                Quality = GetInt(patterns.Quality, propertyBlock),
                Armor = GetInt(patterns.Armor, propertyBlock),
                EnergyShield = GetInt(patterns.EnergyShield, propertyBlock),
                Evasion = GetInt(patterns.Evasion, propertyBlock),
                ChanceToBlock = GetInt(patterns.ChanceToBlock, propertyBlock),
            };
        }

        private Properties ParseAccessoryProperties(ParsingItem parsingItem)
        {
            return new Properties()
            {
                ItemLevel = GetInt(patterns.ItemLevel, parsingItem),
                Identified = !GetBool(patterns.Unidentified, parsingItem),
                Corrupted = GetBool(patterns.Corrupted, parsingItem),
            };
        }

        private Properties ParseMapProperties(ParsingItem parsingItem)
        {
            var propertyBlock = parsingItem.Blocks[1];

            return new Properties()
            {
                ItemLevel = GetInt(patterns.ItemLevel, parsingItem),
                Identified = !GetBool(patterns.Unidentified, parsingItem),
                Corrupted = GetBool(patterns.Corrupted, parsingItem),
                Blighted = patterns.Blighted.IsMatch(parsingItem.Blocks[0].Lines[1].Text),

                ItemQuantity = GetInt(patterns.ItemQuantity, propertyBlock),
                ItemRarity = GetInt(patterns.ItemRarity, propertyBlock),
                MonsterPackSize = GetInt(patterns.MonsterPackSize, propertyBlock),
                MapTier = GetInt(patterns.MapTier, propertyBlock),
                Quality = GetInt(patterns.Quality, propertyBlock),
            };
        }

        private Properties ParseGemProperties(ParsingItem parsingItem)
        {
            var propertyBlock = parsingItem.Blocks[1];

            return new Properties()
            {
                Corrupted = GetBool(patterns.Corrupted, parsingItem),

                GemLevel = GetInt(patterns.Level, propertyBlock),
                Quality = GetInt(patterns.Quality, propertyBlock),
                AlternateQuality = GetBool(patterns.AlternateQuality, parsingItem),
            };
        }

        private Properties ParseJewelProperties(ParsingItem parsingItem)
        {
            return new Properties()
            {
                ItemLevel = GetInt(patterns.ItemLevel, parsingItem),
                Identified = !GetBool(patterns.Unidentified, parsingItem),
                Corrupted = GetBool(patterns.Corrupted, parsingItem),
            };
        }

        private List<Socket> ParseSockets(ParsingItem parsingItem)
        {
            if (TryParseValue(patterns.Socket, parsingItem, out var match))
            {
                var groups = match.Groups
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Skip(1)
                    .Select((x, Index) => new
                    {
                        x.Value,
                        Index,
                    })
                    .ToList();

                var result = new List<Socket>();

                foreach (var group in groups)
                {
                    var groupValue = group.Value.Replace("-", "").Trim();
                    while (groupValue.Length > 0)
                    {
                        switch (groupValue[0])
                        {
                            case 'B': result.Add(new Socket() { Group = group.Index, Colour = SocketColour.Blue }); break;
                            case 'G': result.Add(new Socket() { Group = group.Index, Colour = SocketColour.Green }); break;
                            case 'R': result.Add(new Socket() { Group = group.Index, Colour = SocketColour.Red }); break;
                            case 'W': result.Add(new Socket() { Group = group.Index, Colour = SocketColour.White }); break;
                            case 'A': result.Add(new Socket() { Group = group.Index, Colour = SocketColour.Abyss }); break;
                        }
                        groupValue = groupValue[1..];
                    }
                }

                return result;
            }

            return null;
        }

        private Influences ParseInfluences(ParsingItem parsingItem)
        {
            switch (parsingItem.Metadata.Category)
            {
                case Category.Accessory:
                case Category.Armour:
                case Category.Weapon:
                    return new Influences()
                    {
                        Crusader = GetBool(patterns.Crusader, parsingItem),
                        Elder = GetBool(patterns.Elder, parsingItem),
                        Hunter = GetBool(patterns.Hunter, parsingItem),
                        Redeemer = GetBool(patterns.Redeemer, parsingItem),
                        Shaper = GetBool(patterns.Shaper, parsingItem),
                        Warlord = GetBool(patterns.Warlord, parsingItem),
                    };

                default:
                    return null;
            }
        }

        private ItemModifiers ParseModifiers(ParsingItem parsingItem)
        {
            switch (parsingItem.Metadata.Category)
            {
                case Category.DivinationCard:
                case Category.Currency:
                case Category.Prophecy:
                case Category.Gem:
                    return null;

                default:
                    return modifierProvider.Parse(parsingItem);
            }
        }


        #region Helpers
        private bool GetBool(Regex pattern, ParsingItem parsingItem)
        {
            return TryParseValue(pattern, parsingItem, out var _);
        }

        private int GetInt(Regex pattern, ParsingItem parsingItem)
        {
            if (TryParseValue(pattern, parsingItem, out var match) && int.TryParse(match.Groups[1].Value, out var result))
            {
                return result;
            }

            return default;
        }

        private int GetInt(Regex pattern, ParsingBlock parsingBlock)
        {
            if (TryParseValue(pattern, parsingBlock, out var match) && int.TryParse(match.Groups[1].Value, out var result))
            {
                return result;
            }

            return default;
        }

        private double GetDouble(Regex pattern, ParsingBlock parsingBlock)
        {
            if (TryParseValue(pattern, parsingBlock, out var match) && double.TryParse(match.Groups[1].Value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return default;
        }

        private double GetDps(Regex pattern, ParsingBlock parsingBlock, double attacksPerSecond)
        {
            if (TryParseValue(pattern, parsingBlock, out var match))
            {
                var matches = new Regex("(\\d+-\\d+)").Matches(match.Value);
                var dps = matches
                    .Select(x => x.Value.Split("-"))
                    .Sum(split =>
                    {
                        if (double.TryParse(split[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var minValue)
                         && double.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var maxValue))
                        {
                            return (minValue + maxValue) / 2d;
                        }

                        return 0d;
                    });

                return Math.Round(dps * attacksPerSecond, 2);
            }

            return default;
        }

        private bool TryParseValue(Regex pattern, ParsingItem parsingItem, out Match match)
        {
            foreach (var block in parsingItem.Blocks.Where(x => !x.Parsed))
            {
                if (TryParseValue(pattern, block, out match))
                {
                    return true;
                }
            }

            match = null;
            return false;
        }

        private bool TryParseValue(Regex pattern, ParsingBlock block, out Match match)
        {
            foreach (var line in block.Lines.Where(x => !x.Parsed))
            {
                match = pattern.Match(line.Text);
                if (match.Success)
                {
                    line.Parsed = true;
                    return true;
                }
            }

            match = null;
            return false;
        }

        #endregion Helpers
    }
}
