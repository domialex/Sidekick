using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoPatternGroup
    {
        public PseudoPatternGroup(string id, List<PseudoPattern> patterns)
        {
            Id = id;
            Patterns = patterns;
        }

        public string Id { get; set; }
        public List<PseudoPattern> Patterns { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Text} - {Id}";
        }
    }
}
