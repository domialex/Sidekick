using System.Collections.ObjectModel;
using PropertyChanged;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceModifierCategory
    {
        public string Label { get; set; }

        public ObservableCollection<PriceModifier> Modifiers { get; set; } = new ObservableCollection<PriceModifier>();
    }
}
