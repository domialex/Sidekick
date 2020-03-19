using PropertyChanged;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceFilter
    {
        public bool Enabled { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public bool HasRange { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }
    }
}
