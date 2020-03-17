using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParsedItem : Item
    {
        public int Armor { get; set; } = 0;
        public int EnergyShield { get; set; } = 0;
        public int Evasion { get; set; } = 0;
        public int Quality { get; set; } = 0;
        public int Level { get; set; } = 0;
        public int MapTier { get; set; } = 0;
        public int ItemQuantity { get; set; } = 0;
        public int ItemRarity { get; set; } = 0;
        public int MonsterPackSize { get; set; } = 0;
        public bool Blighted { get; set; } = false;
        public string ElementalDamage { get; set; } = null;
        public double CriticalStrikeChance { get; set; } = 0;
        public double AttacksPerSecond { get; set; } = 0;
        public string PhysicalDamage { get; set; } = null;
    }
}
