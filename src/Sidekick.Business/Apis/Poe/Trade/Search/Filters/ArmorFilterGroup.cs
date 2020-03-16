namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class ArmorFilterGroup
    {
        public bool Disabled { get; set; }
        public ArmorFilter Filters { get; set; } = new ArmorFilter();
    }
}
