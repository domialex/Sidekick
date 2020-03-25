using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParsedItem : Item
    {
        public int Armor { get; set; }
        public int EnergyShield { get; set; }
        public int Evasion { get; set; }
        public int ChanceToBlock { get; set; }
        public int Quality { get; set; }
        public int GemLevel { get; set; }
        public int MapTier { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemRarity { get; set; }
        public int MonsterPackSize { get; set; }
        public bool Blighted { get; set; }
        public double CriticalStrikeChance { get; set; }
        public double AttacksPerSecond { get; set; }

        public string ItemText { get; set; }
    }
}
