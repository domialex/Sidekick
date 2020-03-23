using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoPatternGroup
    {
        public PseudoPatternGroup(string id, Regex exception, List<PseudoPattern> patterns)
        {
            Id = id;
            Exception = exception;
            Patterns = patterns;
        }

        public string Id { get; set; }
        public Regex Exception { get; set; }
        public List<PseudoPattern> Patterns { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Text} - {Id}";
        }
    }
}
