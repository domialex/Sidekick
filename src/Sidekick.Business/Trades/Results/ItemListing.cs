namespace Sidekick.Business.Trades.Results
{
    public class ItemListing
    {
        public string Name { get; set; }
        public string TypeLine { get; set; }
        public bool Verified { get; set; }
        public bool Identified { get; set; }
        public bool Corrupted { get; set; }
        public int Ilvl { get; set; }
    }
}
