namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public ItemFlags Flags { get; set; } = new ItemFlags();
    }
}
