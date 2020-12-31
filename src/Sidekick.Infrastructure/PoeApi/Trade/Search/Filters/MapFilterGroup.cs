namespace Sidekick.Infrastructure.PoeApi.Trade.Search.Filters
{
    public class MapFilterGroup
    {
        public bool Disabled { get; set; }
        public MapFilter Filters { get; set; } = new MapFilter();
    }
}
