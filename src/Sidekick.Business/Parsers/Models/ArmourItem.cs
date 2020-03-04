using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Business.Parsers.Models
{
    public class ArmourItem : EquippableItem
    {
        public string Armour { get; set; }
        public string EnergyShield { get; set; }
        public string Evasion { get; set; }
    }
}
