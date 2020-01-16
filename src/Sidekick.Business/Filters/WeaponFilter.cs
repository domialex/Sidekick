namespace Sidekick.Business.Filters
{
    public class WeaponFilter
    {
        public bool Disabled { get; set; }
        public WeaponFilters Filters { get; set; } = new WeaponFilters();
    }
}
