using PropertyChanged;
using Sidekick.UI.Helpers;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceFilterCategory
    {
        public string Label { get; set; }

        public AsyncObservableCollection<PriceFilter> Filters { get; set; } = new AsyncObservableCollection<PriceFilter>();
    }
}
