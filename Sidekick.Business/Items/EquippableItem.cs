namespace Sidekick.Business.Items
{
    public class EquippableItem : Item
    {
        public string Quality { get; set; }
        public string ItemLevel { get; set; }
        public InfluenceType Influence { get; set; }
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
    }
}
