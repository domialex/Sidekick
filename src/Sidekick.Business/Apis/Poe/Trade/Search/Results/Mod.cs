using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Results
{
    public class Mod
    {
        public string Name { get; set; }
        public string Tier { get; set; }
        public List<Magnitude> Magnitudes { get; set; }
    }
}
