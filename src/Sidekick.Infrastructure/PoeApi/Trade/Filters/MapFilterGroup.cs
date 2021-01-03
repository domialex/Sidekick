namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class MapFilterGroup
    {
        public bool Disabled { get; set; }
        public MapFilter Filters { get; set; } = new MapFilter();
    }
}
