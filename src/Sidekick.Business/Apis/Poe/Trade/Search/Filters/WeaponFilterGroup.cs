namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class WeaponFilterGroup
    {
        public bool Disabled { get; set; }
        public WeaponFilter Filters { get; set; } = new WeaponFilter();
    }
}
