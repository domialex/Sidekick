using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sidekick.Business.Parsers;
using Sidekick.Core.Natives;
using Sidekick.UI.Prices;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Prices
{
    public partial class PriceView : BaseWindow, ISidekickView
    {
        private readonly IPriceViewModel viewModel;
        private readonly INativeBrowser nativeBrowser;

        public PriceView(
            IServiceProvider serviceProvider,
            IPriceViewModel viewModel,
            INativeBrowser nativeBrowser,
            INativeClipboard nativeClipboard,
            IItemParser itemParser)
            : base(serviceProvider)
        {
            Task.Run(async () =>
            {
                var text = await nativeClipboard.Copy();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var item = await itemParser.ParseItem(text);
                    await viewModel.Initialize(item);
                }
            });

            this.viewModel = viewModel;
            this.nativeBrowser = nativeBrowser;

            InitializeComponent();
            DataContext = viewModel;

            Loaded += OverlayWindow_Loaded;

            Show();
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged; ;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();

            //Load next results when scrollviewer is at the bottom
            if (scrollViewer?.ScrollableHeight > 0)
            {
                // Query next page when reaching more than 70% of the scrollable content.
                if ((scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight) > 0.7d)
                {
                    viewModel.LoadMoreData();
                    return;
                }
            }
        }

        public new void Show()
        {
            base.Show();
            var scrollViewer = _itemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
        }

        private void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            nativeBrowser.Open(e.Uri);
            e.Handled = true;
        }
    }
}
