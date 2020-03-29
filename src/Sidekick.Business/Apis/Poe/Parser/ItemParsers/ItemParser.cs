//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using Sidekick.Business.Apis.Poe.Models;
//using Sidekick.Business.Apis.Poe.Parser.Patterns;
//using Sidekick.Business.Apis.Poe.Trade.Data.Stats;

//namespace Sidekick.Business.Apis.Poe.Parser.ItemParsers
//{
//    public abstract class ItemParser
//    {
//        protected readonly IParserPatterns patterns;
//        protected readonly IStatDataService statDataService;

//        public ItemParser(IParserPatterns patterns, IStatDataService statDataService)
//        {
//            this.patterns = patterns;
//            this.statDataService = statDataService;
//        }

//        public abstract ParsedItem ParseItem();

//        protected void ParseProperties(ref ParsedItem item, ref string input)
//        {
//            var blocks = separatorPattern.Split(input);

//            item.Armor = GetInt(patterns.Armor, blocks[1]);
//            item.EnergyShield = GetInt(patterns.EnergyShield, blocks[1]);
//            item.Evasion = GetInt(patterns.Evasion, blocks[1]);
//            item.ChanceToBlock = GetInt(patterns.ChanceToBlock, blocks[1]);
//            item.Quality = GetInt(patterns.Quality, blocks[1]);
//            item.MapTier = GetInt(patterns.MapTier, blocks[1]);
//            item.ItemQuantity = GetInt(patterns.ItemQuantity, blocks[1]);
//            item.ItemRarity = GetInt(patterns.ItemRarity, blocks[1]);
//            item.MonsterPackSize = GetInt(patterns.MonsterPackSize, blocks[1]);
//            item.AttacksPerSecond = GetDouble(patterns.AttacksPerSecond, blocks[1]);
//            item.CriticalStrikeChance = GetDouble(patterns.CriticalStrikeChance, blocks[1]);
//            item.Extended.ElementalDps = GetDps(patterns.ElementalDamage, blocks[1], item.AttacksPerSecond);
//            item.Extended.PhysicalDps = GetDps(patterns.PhysicalDamage, blocks[1], item.AttacksPerSecond);
//            item.Extended.DamagePerSecond = item.Extended.ElementalDps + item.Extended.PhysicalDps;
//            item.Blighted = patterns.Blighted.IsMatch(blocks[0]);

//            if (item.Rarity == Rarity.Gem)
//            {
//                item.GemLevel = GetInt(patterns.Level, blocks[1]);
//            }
//        }

//        protected void ParseSockets(ref ParsedItem item, ref string input)
//        {
//            var result = patterns.Socket.Match(input);
//            if (result.Success)
//            {
//                var groups = result.Groups
//                    .Where(x => !string.IsNullOrEmpty(x.Value))
//                    .Select(x => x.Value)
//                    .ToList();

//                for (var index = 1; index < groups.Count; index++)
//                {
//                    var groupValue = groups[index].Replace("-", "").Trim();
//                    while (groupValue.Length > 0)
//                    {
//                        switch (groupValue[0])
//                        {
//                            case 'B': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Blue }); break;
//                            case 'G': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Green }); break;
//                            case 'R': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Red }); break;
//                            case 'W': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.White }); break;
//                            case 'A': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Abyss }); break;
//                        }
//                        groupValue = groupValue.Substring(1);
//                    }
//                }
//            }
//        }

//        protected void ParseInfluences(ref ParsedItem item, ref string input)
//        {
//            var blocks = separatorPattern.Split(input);
//            var strippedInput = string.Concat(blocks.Skip(1).ToList());

//            item.Influences.Crusader = patterns.Crusader.IsMatch(strippedInput);
//            item.Influences.Elder = patterns.Elder.IsMatch(strippedInput);
//            item.Influences.Hunter = patterns.Hunter.IsMatch(strippedInput);
//            item.Influences.Redeemer = patterns.Redeemer.IsMatch(strippedInput);
//            item.Influences.Shaper = patterns.Shaper.IsMatch(strippedInput);
//            item.Influences.Warlord = patterns.Warlord.IsMatch(strippedInput);
//        }

//        protected void ParseMods(ref ParsedItem item, ref string input)
//        {
//            item.Extended.Mods = statDataService.ParseMods(input);
//        }

//        #region Helpers
//        protected int GetInt(Regex regex, string input)
//        {
//            if (regex != null)
//            {
//                var match = regex.Match(input);

//                if (match.Success)
//                {
//                    if (int.TryParse(match.Groups[1].Value, out var result))
//                    {
//                        return result;
//                    }
//                }
//            }

//            return 0;
//        }

//        protected double GetDouble(Regex regex, string input)
//        {
//            if (regex != null)
//            {
//                var match = regex.Match(input);

//                if (match.Success)
//                {
//                    if (double.TryParse(match.Groups[1].Value.Replace(",", "."), out var result))
//                    {
//                        return result;
//                    }
//                }
//            }

//            return 0;
//        }

//        protected double GetDps(Regex regex, string input, double attacksPerSecond)
//        {
//            if (regex != null)
//            {
//                var match = regex.Match(input);

//                if (match.Success)
//                {
//                    var split = match.Groups[1].Value.Split('-');

//                    if (int.TryParse(split[0], out var minValue) && int.TryParse(split[1], out var maxValue))
//                    {
//                        return ((minValue + maxValue) / 2) * attacksPerSecond;
//                    }
//                }
//            }

//            return 0;
//        }
//        #endregion
//    }
//}
