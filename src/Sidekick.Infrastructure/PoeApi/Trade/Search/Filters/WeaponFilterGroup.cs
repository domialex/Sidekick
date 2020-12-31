namespace Sidekick.Infrastructure.PoeApi.Trade.Search.Filters
{
    public class WeaponFilterGroup
    {
        public bool Disabled { get; set; }
        public WeaponFilter Filters { get; set; } = new WeaponFilter();
    }
}
