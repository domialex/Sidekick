namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class TypeFilterGroup
    {
        public bool Disabled { get; set; }
        public TypeFilter Filters { get; set; } = new TypeFilter();
    }
}
