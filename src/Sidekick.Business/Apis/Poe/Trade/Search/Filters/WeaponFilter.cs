namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class WeaponFilter
    {
        public FilterValue Damage { get; set; }
        public FilterValue Crit { get; set; }
        public FilterValue APS { get; set; }
        public FilterValue EDPS { get; set; }
        public FilterValue PDPS { get; set; }
    }
}
