using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoPattern
    {
        public PseudoPattern(string regex, int multiplier = 1)
        {
            Pattern = new Regex(regex);
            Multiplier = multiplier;
        }

        public Regex Pattern { get; set; }

        public int Multiplier { get; set; }

        public List<PseudoPatternMatch> Matches { get; set; } = new List<PseudoPatternMatch>();

    }
}
