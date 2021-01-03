namespace Sidekick.Domain.Game.Trade.Models
{
    public class PropertyFilter
    {
        public PropertyFilterType Type { get; set; }

        public bool Enabled { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }
    }
}
