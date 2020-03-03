using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.UI.Prices
{
    public interface IPriceViewModel
    {
        bool IsError { get; }
        bool IsNotError { get; }

        bool IsFetching { get; }
        bool IsFetched { get; }
        Item Item { get; }
        string ItemColor { get; }
        string CountString { get; }

        Uri Uri { get; }

        bool IsPoeNinja { get; }
        string PoeNinjaText { get; }

        bool IsPredicted { get; }
        string PredictionText { get; }

        ObservableCollection<PriceItem> Results { get; }

        Task LoadMoreData();
    }
}
