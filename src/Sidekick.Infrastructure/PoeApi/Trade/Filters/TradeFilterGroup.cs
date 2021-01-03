namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class TradeFilterGroup
    {
        public bool Disabled { get; set; }
        public TradeFilter Filters { get; set; } = new TradeFilter();
    }
}
