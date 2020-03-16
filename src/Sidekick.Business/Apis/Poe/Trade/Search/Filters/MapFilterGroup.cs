namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class MapFilterGroup
    {
        public bool Disabled { get; set; }
        public MapFilter Filters { get; set; } = new MapFilter();
    }
}
