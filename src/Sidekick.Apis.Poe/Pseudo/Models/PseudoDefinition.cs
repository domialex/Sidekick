using System.Collections.Generic;

namespace Sidekick.Apis.Poe.Pseudo.Models
{
    public class PseudoDefinition
    {
        public PseudoDefinition(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }

        public List<PseudoDefinitionModifier> Modifiers { get; set; } = new List<PseudoDefinitionModifier>();

        public override string ToString()
        {
            return $"{Text} - {Id}";
        }
    }
}
