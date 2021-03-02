using System.Text.RegularExpressions;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class ModifierPattern
    {
        public string Text { get; set; }

        public int LineCount { get; set; }

        public Regex Pattern { get; set; }

        public bool Negated { get; set; } = false;

        public int? Value { get; set; }
    }
}
