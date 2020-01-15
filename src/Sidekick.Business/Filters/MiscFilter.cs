namespace Sidekick.Business.Filters
{
    public class MiscFilter
    {
        public bool Disabled { get; set; }
        public MiscFilters Filters { get; set; } = new MiscFilters();
    }
}
