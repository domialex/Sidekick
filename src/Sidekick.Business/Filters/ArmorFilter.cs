namespace Sidekick.Business.Filters
{
    public class ArmorFilter
    {
        public bool Disabled { get; set; }
        public ArmorFilters Filters { get; set; } = new ArmorFilters();
    }
}
