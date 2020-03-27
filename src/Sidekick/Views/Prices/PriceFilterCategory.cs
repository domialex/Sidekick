using System.Collections.ObjectModel;
using PropertyChanged;

namespace Sidekick.Views.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceFilterCategory
    {
        public string Label { get; set; }

        public ObservableCollection<PriceFilter> Filters { get; set; } = new ObservableCollection<PriceFilter>();
    }
}
