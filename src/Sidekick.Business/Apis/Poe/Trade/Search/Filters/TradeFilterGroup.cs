namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class TradeFilterGroup
    {
        public bool Disabled { get; set; }
        public TradeFilter Filters { get; set; } = new TradeFilter();
    }
}
