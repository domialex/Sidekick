using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class PropertyFilters
    {
        public Class? Class { get; set; } = null;
        public List<PropertyFilter> Weapon { get; set; } = new();
        public List<PropertyFilter> Armour { get; set; } = new();
        public List<PropertyFilter> Map { get; set; } = new();
        public List<PropertyFilter> Misc { get; set; } = new();
    }
}
