using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class ModifierFilter
    {
        public Modifier Modifier { get; set; }

        public bool Enabled { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }
    }
}
