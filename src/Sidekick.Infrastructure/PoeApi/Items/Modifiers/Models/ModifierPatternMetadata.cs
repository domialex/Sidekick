using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class ModifierPatternMetadata
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public List<ModifierPattern> Patterns { get; set; }

        public List<ModifierOptionParse> Options { get; set; }
    }
}
