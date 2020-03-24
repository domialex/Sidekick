using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoDefinitionModifier
    {
        public PseudoDefinitionModifier(string type, string text, double multiplier)
        {
            Type = type;
            Text = text;
            Multiplier = multiplier;
        }

        public List<string> Ids { get; set; } = new List<string>();

        public string Type { get; set; }

        public string Text { get; set; }

        public double Multiplier { get; set; }

        public override string ToString()
        {
            return $"{Text} - {Multiplier}x - {string.Join(", ", Ids)} ({Ids.Count})";
        }
    }
}
