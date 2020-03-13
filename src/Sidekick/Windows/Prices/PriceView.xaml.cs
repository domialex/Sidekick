using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sidekick.Core.Natives;
using Sidekick.UI.Prices;

namespace Sidekick.Windows.Prices
{
    public partial class PriceView : BaseWindow
    {
        private readonly IPriceViewModel viewModel;
        private readonly INativeBrowser nativeBrowser;

        public PriceView(
            IServiceProvider serviceProvider,
            IPriceViewModel viewModel,
            INativeBrowser nativeBrowser,
            INativeCursor cursor)
            : base(serviceProvider)
        {
            this.viewModel = viewModel;
            this.nativeBrowser = nativeBrowser;
            InitializeComponent();
            DataContext = viewModel;

            Loaded += OverlayWindow_Loaded;

            Show();

            var position = cursor.GetCursorPosition();
            SetWindowPositionFromBottomRight(position.X - 10, position.Y - 10);

            if (viewModel.IsError)
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(1500);
                    Close();
                });
            }
        }

        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = ItemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = ItemList.GetChildOfType<ScrollViewer>();

            // Load next results when scrollviewer is at the bottom
            if (scrollViewer?.ScrollableHeight > 0)
            {
                // Query next page when reaching more than 80% of the scrollable content.
                if ((scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight) > 0.8d)
                {
                    viewModel.LoadMoreData();
                }
            }
        }

        public new void Show()
        {
            base.Show();
            var scrollViewer = ItemList.GetChildOfType<ScrollViewer>();
            scrollViewer?.ScrollToTop();
        }

        private void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            nativeBrowser.Open(e.Uri);
            e.Handled = true;
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.Preview((PriceItem)ItemList.SelectedItem);
            EnsureBounds();
        }
    }
}
