namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class RequirementFilterGroup
    {
        public bool Disabled { get; set; }
        public RequirementFilter Filters { get; set; } = new RequirementFilter();
    }
}
