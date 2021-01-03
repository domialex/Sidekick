namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class WeaponFilterGroup
    {
        public bool Disabled { get; set; }
        public WeaponFilter Filters { get; set; } = new WeaponFilter();
    }
}
