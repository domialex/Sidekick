using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.UI.Prices
{
    public interface IPriceViewModel
    {
        bool IsError { get; }
        bool IsNotError { get; }

        bool IsFetching { get; }
        bool IsFetched { get; }
        ParsedItem Item { get; }
        string ItemColor { get; }
        string CountString { get; }

        Uri Uri { get; }

        bool IsPoeNinja { get; }
        string PoeNinjaText { get; }

        bool IsPredicted { get; }
        string PredictionText { get; }

        ObservableCollection<PriceItem> Results { get; }

        Task LoadMoreData();

        bool HasPreviewItem { get; }
        PriceItem PreviewItem { get; }
        void Preview(PriceItem selectedItem);

        void UpdateQuery();
    }
}
