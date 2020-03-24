using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
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
