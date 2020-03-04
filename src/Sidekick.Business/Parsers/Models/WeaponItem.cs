using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Business.Parsers.Models
{
    public class WeaponItem : EquippableItem
    {
        public string PhysicalDps { get; set; }
        public string ElementalDps { get; set; }
        public string AttacksPerSecond { get; set; }
        public string CriticalStrikeChance { get; set; }
    }
}
