using PropertyChanged;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceModifier
    {
        public bool Enabled { get; set; }

        public string Text { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }
    }
}
