namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class RequirementFilterGroup
    {
        public bool Disabled { get; set; }
        public RequirementFilter Filters { get; set; } = new RequirementFilter();
    }
}
