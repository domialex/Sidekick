using System.Collections.Generic;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class ModifierFilters
    {
        public List<ModifierFilter> Implicit { get; set; } = new();
        public List<ModifierFilter> Explicit { get; set; } = new();
        public List<ModifierFilter> Crafted { get; set; } = new();
        public List<ModifierFilter> Enchant { get; set; } = new();
        public List<ModifierFilter> Pseudo { get; set; } = new();
        public List<ModifierFilter> Fractured { get; set; } = new();
    }
}
