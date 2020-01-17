namespace Sidekick.Business.Filters
{
    public class MapFilter
    {
        public bool Disabled { get; set; }
        public MapFilters Filters { get; set; } = new MapFilters();
    }
}
