using System.Collections.Generic;

namespace Sidekick.Business.Trades.Results
{
    public class Item
    {
        public bool Verified { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string Icon { get; set; }
        public string League { get; set; }
        public List<Socket> Sockets { get; set; }
        public string Name { get; set; }
        public string TypeLine { get; set; }
        public bool Identified { get; set; }
        public int Ilvl { get; set; }
        public string Note { get; set; }
        public List<Property> Properties { get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<string> ImplicitMods { get; set; }
        public List<string> ExplicitMods { get; set; }
        public string DescrText { get; set; }
        public List<string> FlavourText { get; set; }
        public int FrameType { get; set; }
        public bool Corrupted { get; set; }
        public Extended Extended { get; set; }
    }
}
