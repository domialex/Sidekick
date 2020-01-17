namespace Sidekick.Business.Filters
{
    public class TypeFilter
    {
        public bool Disabled { get; set; }
        public TypeFilters Filters { get; set; } = new TypeFilters();
    }
}
