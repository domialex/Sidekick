namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class SocketFilterGroup
    {
        public bool Disabled { get; set; }
        public SocketFilter Filters { get; set; } = new SocketFilter();
    }
}
