using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser.Patterns
{
    public class ParserPatterns : IParserPatterns
    {
        #region Header (Rarity, Name, Type)
        public Dictionary<Rarity, Regex> Rarity { get; set; }
        public Regex ItemLevel { get; set; }
        public Regex Unidentified { get; set; }
        public Regex Corrupted { get; set; }
        #endregion Header (Rarity, Name, Type)

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)
        public Regex Armor { get; set; }
        public Regex EnergyShield { get; set; }
        public Regex Evasion { get; set; }
        public Regex ChanceToBlock { get; set; }
        public Regex Quality { get; set; }
        public Regex AlternateQuality { get; set; }
        public Regex Level { get; set; }
        public Regex MapTier { get; set; }
        public Regex ItemQuantity { get; set; }
        public Regex ItemRarity { get; set; }
        public Regex MonsterPackSize { get; set; }
        public Regex AttacksPerSecond { get; set; }
        public Regex CriticalStrikeChance { get; set; }
        public Regex ElementalDamage { get; set; }
        public Regex PhysicalDamage { get; set; }
        public Regex Blighted { get; set; }
        #endregion Properties (Armour, Evasion, Energy Shield, Quality, Level)

        #region Sockets
        public Regex Socket { get; set; }
        #endregion Sockets

        #region Influences
        public Regex Crusader { get; set; }
        public Regex Elder { get; set; }
        public Regex Hunter { get; set; }
        public Regex Redeemer { get; set; }
        public Regex Shaper { get; set; }
        public Regex Warlord { get; set; }
        #endregion Influences

        #region Helpers
        public int GetInt(Regex regex, string input)
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

        public double GetDouble(Regex regex, string input)
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

        public double GetDps(Regex regex, string input, double attacksPerSecond)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    var matches = new Regex("(\\d+-\\d+)").Matches(match.Value);
                    var dps = matches.Select(x => x.Value.Split("-"))
                                     .ToList()
                                     .Sum(split =>
                    {
                        if (double.TryParse(split[0], out var minValue)
                         && double.TryParse(split[1], out var maxValue))
                        {
                            return (minValue + maxValue) / 2d;
                        }

                        return 0d;
                    });

                    return Math.Round(dps * attacksPerSecond, 2);
                }
            }

            return 0d;
        }
        #endregion Helpers
    }
}
