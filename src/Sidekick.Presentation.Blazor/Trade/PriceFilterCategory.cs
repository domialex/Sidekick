using System.Collections.ObjectModel;

namespace Sidekick.Presentation.Blazor.Trade
{
    public class PriceFilterCategory
    {
        public ObservableCollection<PriceFilter> Filters { get; set; } = new ObservableCollection<PriceFilter>();
    }
}
