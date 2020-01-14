namespace Sidekick.Business.Items
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IsCorrupted { get; set; }
        public string Rarity { get; set; }
    }
}
