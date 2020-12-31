namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class SocketFilterGroup
    {
        public bool Disabled { get; set; }
        public SocketFilter Filters { get; set; } = new SocketFilter();
    }
}
