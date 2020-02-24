using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Categories;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Natives;
using Sidekick.UI.Prices;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Prices
{
    public partial class OverlayWindow : BaseWindow, ISidekickView
    {
        #region Events
        public delegate void ItemScrollReachedEndHandler(Business.Parsers.Models.Item item, int displayedItemsCount);
        public event ItemScrollReachedEndHandler ItemScrollReachedEnd;
        public void OnItemScrollReachedEnd(Business.Parsers.Models.Item item, int displayedItemsCount)
        {

            var page = (int)Math.Ceiling(displayedItemsCount / 10d);
            var queryResult = await tradeClient.GetListingsForSubsequentPages(item, page);
            if (queryResult.Result.Any())
            {
                overlayWindow.AppendQueryResult(queryResult);
            }

            ItemScrollReachedEnd?.Invoke(item, displayedItemsCount);
        }
        #endregion

        private readonly IPriceViewModel viewModel;
        private readonly IPoePriceInfoClient poePriceInfoClient;
        private readonly INativeBrowser nativeBrowser;
        private readonly INativeClipboard nativeClipboard;
        private readonly IItemParser itemParser;
        private readonly ILanguageProvider languageProvider;
        private readonly IStaticItemCategoryService staticItemCategoryService;

        public OverlayWindow(
            IServiceProvider serviceProvider,
            IPriceViewModel viewModel,
            IPoePriceInfoClient poePriceInfoClient,
            INativeBrowser nativeBrowser,
            INativeClipboard nativeClipboard,
            IItemParser itemParser,
            ILanguageProvider languageProvider,
            IStaticItemCategoryService staticItemCategoryService)
            : base(serviceProvider)
        {
            Task.Run(async () =>
            {
                var text = await nativeClipboard.Copy();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var item = await itemParser.ParseItem(text);

                    if (item != null)
                    {
                        Open();

                        var queryResult = await tradeClient.GetListings(item);
                        if (queryResult != null)
                        {
                            var poeNinjaItem = poeNinjaCache.GetItem(item);
                            if (poeNinjaItem != null)
                            {
                                queryResult.PoeNinjaItem = poeNinjaItem;
                                queryResult.LastRefreshTimestamp = poeNinjaCache.LastRefreshTimestamp;
                            }

                            SetQueryResult(queryResult);
                            return true;
                        }

                        Hide();
                        return true;
                    }
                }
            });

            this.viewModel = viewModel;
            this.poePriceInfoClient = poePriceInfoClient;
            this.nativeBrowser = nativeBrowser;
            this.nativeClipboard = nativeClipboard;
            this.itemParser = itemParser;
            this.languageProvider = languageProvider;
            this.staticItemCategoryService = staticItemCategoryService;
            InitializeComponent();
            DataContext = this;
            Hide();
            Loaded += OverlayWindow_Loaded;
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
            scrollViewer.ScrollChanged += itemList_ScrollChanged;
        }

        public void SetQueryResult(QueryResult<SearchResult> queryResult)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetQueryResultCallback(SetQueryResult), new object[] { queryResult });
            }
            else
            {
                AppendToItemListing(queryResult.Result);

                // Hardcoded to the English value of Rare since poeprices.info only support English.
                if (queryResult.Item.Rarity == Business.Parsers.Models.Rarity.Rare && queryResult.Item.IsIdentified)
                {
                    Task.Run(() => GetPricePrediction(queryResult.Item.ItemText));
                }
            }
        }
        delegate void SetQueryResultCallback(QueryResult<SearchResult> queryToAppend);

        delegate void AppendQueryResultCallback(QueryResult<SearchResult> queryResult);

        public void SetWindowPosition(int x, int y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetWindowPositionCallback(SetWindowPosition), new object[] { x, y });
            }
            else
            {
                Left = x;
                Top = y;
            }
        }
        delegate void SetWindowPositionCallback(int x, int y);

        public new void Show()
        {
            base.Show();
            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
        }

        public void HideWindowAndClearData()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HideWindowAndClearDataCallback(HideWindowAndClearData));
            }
            else
            {
                txtPrediction.Text = null;
                queryResult = null;
                itemListingControls = new ObservableCollection<ListItem>();
                NotifyPropertyChanged("itemListingControls");
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowAndClearDataCallback();

        private void itemList_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            // The api only returns 100 results maximum.
            if (!itemListingControls.Any() || itemListingControls.Count >= 100)
            {
                return;
            }

            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();

            //Load next results when scrollviewer is at the bottom
            if (scrollViewer?.ScrollableHeight > 0)
            {
                // Query next page when reaching more than 70% of the scrollable content.
                var breakpoint = (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight) > 0.7d;
                if (breakpoint && overlayIsUpdatable && !dataIsUpdating)
                {
                    dataIsUpdating = true;
                    overlayIsUpdatable = false;
                    OnItemScrollReachedEnd(queryResult.Item, itemListingControls.Count);
                    return;
                }
            }

            // UI update is finished, when the scrollviewer is reset (newly added items will move the scrollbar)
            overlayIsUpdatable = true;
        }

        private void openQueryLink(object sender, RequestNavigateEventArgs e)
        {
            nativeBrowser.Open(e.Uri);
            e.Handled = true;
        }
    }
}
