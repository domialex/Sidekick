namespace Sidekick.Business.Filters
{
    public class RequierementFilter
    {
        public bool Disabled { get; set; }
        public RequierementFilters Filters { get; set; } = new RequierementFilters();
    }
}
