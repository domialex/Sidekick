namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class WeaponFilter
    {
        public SearchFilterValue Damage { get; set; }
        public SearchFilterValue Crit { get; set; }
        public SearchFilterValue APS { get; set; }
        public SearchFilterValue EDPS { get; set; }
        public SearchFilterValue PDPS { get; set; }
    }
}
