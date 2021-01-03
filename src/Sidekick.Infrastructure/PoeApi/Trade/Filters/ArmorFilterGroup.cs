namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class ArmorFilterGroup
    {
        public bool Disabled { get; set; }
        public ArmorFilter Filters { get; set; } = new ArmorFilter();
    }
}
