using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParsedItem : Item
    {
        public int Armor { get; set; } = 0;
        public int EnergyShield { get; set; } = 0;
        public int Evasion { get; set; } = 0;
    }
}
