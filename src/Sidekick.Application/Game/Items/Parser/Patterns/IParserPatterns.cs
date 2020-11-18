using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Application.Game.Items.Parser.Patterns
{
    public interface IParserPatterns
    {
        Regex Armor { get; set; }
        Regex AttacksPerSecond { get; set; }
        Regex Blighted { get; set; }
        Regex ChanceToBlock { get; set; }
        Regex Corrupted { get; set; }
        Regex CriticalStrikeChance { get; set; }
        Regex Crusader { get; set; }
        Regex Elder { get; set; }
        Regex ElementalDamage { get; set; }
        Regex EnergyShield { get; set; }
        Regex Evasion { get; set; }
        Regex Hunter { get; set; }
        Regex ItemLevel { get; set; }
        Regex ItemQuantity { get; set; }
        Regex ItemRarity { get; set; }
        Regex Level { get; set; }
        Regex MapTier { get; set; }
        Regex MonsterPackSize { get; set; }
        Regex PhysicalDamage { get; set; }
        Regex Quality { get; set; }
        Regex AlternateQuality { get; set; }
        Dictionary<Rarity, Regex> Rarity { get; set; }
        Regex Redeemer { get; set; }
        Regex Shaper { get; set; }
        Regex Socket { get; set; }
        Regex Unidentified { get; set; }
        Regex Warlord { get; set; }

        int GetInt(Regex regex, string input);

        double GetDouble(Regex regex, string input);

        double GetDps(Regex regex, string input, double attacksPerSecond);
    }
}
