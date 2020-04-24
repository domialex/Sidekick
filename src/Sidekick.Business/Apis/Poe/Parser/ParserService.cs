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

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IParserService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IStatDataService statsDataService;
        private readonly IItemDataService itemDataService;
        private readonly IParserPatterns patterns;

        private readonly Regex NewlinePattern;
        private readonly Regex SeparatorPattern;

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

            NewlinePattern = new Regex("[\\r\\n]+");
            SeparatorPattern = new Regex("--------");
        }

        public async Task<Item> ParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                itemText = new ItemNameTokenizer().CleanString(itemText);
                var item = new Item
                {
                    Text = itemText
                };

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

        private void ParseHeader(ref Item item, ref string input)
        {
            var dataItem = itemDataService.ParseItem(input);

            if (dataItem == null)
            {
                throw new NotSupportedException("Item not found.");
            }

            item.Name = dataItem.Name;
            item.Type = dataItem.Type;
            item.Rarity = dataItem.Rarity;

            var lines = NewlinePattern.Split(input);
            item.NameLine = SeparatorPattern.IsMatch(lines[1]) ? string.Empty : lines[1];
            item.TypeLine = SeparatorPattern.IsMatch(lines[2]) ? string.Empty : lines[2];

            if (item.Rarity == Rarity.Unknown)
            {
                item.Rarity = GetRarity(lines[0]);
            }

            if (item.Rarity != Rarity.DivinationCard)
            {
                item.ItemLevel = patterns.GetInt(patterns.ItemLevel, input);
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

        private void ParseProperties(ref Item item, ref string input)
        {
            var blocks = SeparatorPattern.Split(input);

            item.Properties.Armor = patterns.GetInt(patterns.Armor, blocks[1]);
            item.Properties.EnergyShield = patterns.GetInt(patterns.EnergyShield, blocks[1]);
            item.Properties.Evasion = patterns.GetInt(patterns.Evasion, blocks[1]);
            item.Properties.ChanceToBlock = patterns.GetInt(patterns.ChanceToBlock, blocks[1]);
            item.Properties.Quality = patterns.GetInt(patterns.Quality, blocks[1]);
            item.Properties.MapTier = patterns.GetInt(patterns.MapTier, blocks[1]);
            item.Properties.ItemQuantity = patterns.GetInt(patterns.ItemQuantity, blocks[1]);
            item.Properties.ItemRarity = patterns.GetInt(patterns.ItemRarity, blocks[1]);
            item.Properties.MonsterPackSize = patterns.GetInt(patterns.MonsterPackSize, blocks[1]);
            item.Properties.AttacksPerSecond = patterns.GetDouble(patterns.AttacksPerSecond, blocks[1]);
            item.Properties.CriticalStrikeChance = patterns.GetDouble(patterns.CriticalStrikeChance, blocks[1]);
            item.Properties.ElementalDps = patterns.GetDps(patterns.ElementalDamage, blocks[1], item.Properties.AttacksPerSecond);
            item.Properties.PhysicalDps = patterns.GetDps(patterns.PhysicalDamage, blocks[1], item.Properties.AttacksPerSecond);
            item.Properties.DamagePerSecond = item.Properties.ElementalDps + item.Properties.PhysicalDps;
            item.Properties.Blighted = patterns.Blighted.IsMatch(blocks[0]);

            if (item.Rarity == Rarity.Gem)
            {
                item.Properties.GemLevel = patterns.GetInt(patterns.Level, blocks[1]);
            }
        }

        private void ParseSockets(ref Item item, ref string input)
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

        private void ParseInfluences(ref Item item, ref string input)
        {
            var blocks = SeparatorPattern.Split(input);
            var strippedInput = string.Concat(blocks.Skip(1).ToList());

            item.Influences.Crusader = patterns.Crusader.IsMatch(strippedInput);
            item.Influences.Elder = patterns.Elder.IsMatch(strippedInput);
            item.Influences.Hunter = patterns.Hunter.IsMatch(strippedInput);
            item.Influences.Redeemer = patterns.Redeemer.IsMatch(strippedInput);
            item.Influences.Shaper = patterns.Shaper.IsMatch(strippedInput);
            item.Influences.Warlord = patterns.Warlord.IsMatch(strippedInput);
        }

        private void ParseMods(ref Item item, ref string input)
        {
            item.Modifiers = statsDataService.ParseMods(input);
        }

    }
}
