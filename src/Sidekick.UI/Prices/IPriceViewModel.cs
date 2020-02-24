using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.UI.Prices
{
    public interface IPriceViewModel
    {
        bool IsFetching { get; }
        Item Item { get; }
        string ItemColor { get; }
        PoeNinjaItem PoeNinjaItem { get; }
        DateTime? PoeNinjaLastRefreshTimestamp { get; }
        string PredictionText { get; }
        ObservableCollection<PriceItem> Results { get; }

        Task Initialize(Item item);
        Task LoadMoreData();
    }
}
