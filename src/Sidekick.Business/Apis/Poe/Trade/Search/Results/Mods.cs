using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Results
{
    public class Mods
    {
        public List<Mod> Implicit { get; set; }
        public List<Mod> Explicit { get; set; }
        public List<Mod> Crafted { get; set; }
    }
}
