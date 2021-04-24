using System.Collections.Generic;

namespace Sidekick.Domain.Game.Modifiers.Models
{
    public class Modifier
    {
        public int Index { get; set; }

        public string Id { get; set; }

        public string Tier { get; set; }

        public string TierName { get; set; }

        public string Category { get; set; }

        public string Text { get; set; }

        public List<double> Values { get; set; } = new List<double>();

        public ModifierOption OptionValue { get; set; }
    }
}
