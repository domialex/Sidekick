using System.Collections.Generic;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class PropertyFilters
    {
        public List<PropertyFilter> Weapon { get; set; } = new();
        public List<PropertyFilter> Armour { get; set; } = new();
        public List<PropertyFilter> Map { get; set; } = new();
        public List<PropertyFilter> Misc { get; set; } = new();
    }
}
