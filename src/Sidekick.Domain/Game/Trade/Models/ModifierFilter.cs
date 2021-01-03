namespace Sidekick.Domain.Game.Trade.Models
{
    public class ModifierFilter
    {
        public string Id { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public object Value { get; set; }
    }
}
