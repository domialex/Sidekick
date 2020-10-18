using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser.Patterns
{
    public interface IParserPatterns
    {
        Regex Armor { get; }
        Regex AttacksPerSecond { get; }
        Regex Blighted { get; }
        Regex ChanceToBlock { get; }
        Regex Corrupted { get; }
        Regex CriticalStrikeChance { get; }
        Regex Crusader { get; }
        Regex Elder { get; }
        Regex ElementalDamage { get; }
        Regex EnergyShield { get; }
        Regex Evasion { get; }
        Regex Hunter { get; }
        Regex ItemLevel { get; }
        Regex ItemQuantity { get; }
        Regex ItemRarity { get; }
        Regex Level { get; }
        Regex MapTier { get; }
        Regex MonsterPackSize { get; }
        Regex PhysicalDamage { get; }
        Regex Quality { get; }
        Regex AlternateQuality { get; }
        Dictionary<Rarity, Regex> Rarity { get; }
        Regex Redeemer { get; }
        Regex Shaper { get; }
        Regex Socket { get; }
        Regex Unidentified { get; }
        Regex Warlord { get; }

        int GetInt(Regex regex, string input);

        double GetDouble(Regex regex, string input);

        double GetDps(Regex regex, string input, double attacksPerSecond);
    }
}
