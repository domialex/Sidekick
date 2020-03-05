using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Business.Parsers.Models
{
    public class WeaponItem : EquippableItem
    {
        public string PhysicalDamage { get; set; }
        public string ElementalDamage { get; set; }
        public string AttacksPerSecond { get; set; }
        public string CriticalStrikeChance { get; set; }
    }
}
