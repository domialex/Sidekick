using PropertyChanged;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceModifier
    {
        public bool Enabled { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }
    }
}
