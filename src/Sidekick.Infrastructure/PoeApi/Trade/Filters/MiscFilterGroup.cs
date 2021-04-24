namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class MiscFilterGroup
    {
        public bool Disabled { get; set; }
        public MiscFilter Filters { get; set; } = new MiscFilter();
    }
}
