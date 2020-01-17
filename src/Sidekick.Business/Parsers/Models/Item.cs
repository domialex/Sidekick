namespace Sidekick.Business.Parsers.Models
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsCorrupted { get; set; }
        public string Rarity { get; set; }
    }
}
