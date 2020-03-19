namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class MiscFilterGroup
    {
        public bool Disabled { get; set; }
        public MiscFilter Filters { get; set; } = new MiscFilter();
    }
}
