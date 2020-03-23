using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoPattern
    {
        public PseudoPattern(Regex regex, double multiplier = 1)
        {
            Pattern = regex;
            Multiplier = multiplier;
        }

        public Regex Pattern { get; set; }

        public double Multiplier { get; set; }

        public List<PseudoPatternMatch> Matches { get; set; } = new List<PseudoPatternMatch>();

    }
}
