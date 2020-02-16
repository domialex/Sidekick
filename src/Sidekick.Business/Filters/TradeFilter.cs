namespace Sidekick.Business.Filters
{
    public class TradeFilter
    {
        public bool Disabled { get; set; }
        public TradeFilters Filters { get; set; } = new TradeFilters();
    }
}
