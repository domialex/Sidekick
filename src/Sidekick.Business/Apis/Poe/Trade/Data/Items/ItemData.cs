namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public ItemDataFlags Flags { get; set; } = new ItemDataFlags();
    }
}
