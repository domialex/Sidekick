using System.Collections.ObjectModel;
using PropertyChanged;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceFilterCategory
    {
        public ObservableCollection<PriceFilter> Filters { get; set; } = new ObservableCollection<PriceFilter>();
    }
}
