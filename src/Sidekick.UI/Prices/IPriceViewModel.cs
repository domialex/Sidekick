using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.UI.Prices
{
    public interface IPriceViewModel : INotifyPropertyChanged
    {
        bool IsFetching { get; }
        Item Item { get; }
        PoeNinjaItem PoeNinjaItem { get; }
        DateTime? PoeNinjaLastRefreshTimestamp { get; }
        string PredictionText { get; }
        ObservableCollection<PriceItem> Results { get; }

        Task Initialize(Item item);
        Task LoadMoreData();
    }
}
