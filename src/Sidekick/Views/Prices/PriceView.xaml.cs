using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sidekick.Core.Natives;

namespace Sidekick.Views.Prices
{
    public partial class PriceView : BaseWindow
    {
        private readonly PriceViewModel viewModel;
        private readonly INativeBrowser nativeBrowser;

        private bool MoveWhenOpeningPreview = false;

        public PriceView(
            IServiceProvider serviceProvider,
            PriceViewModel viewModel,
            INativeBrowser nativeBrowser,
            INativeCursor cursor)
            : base(serviceProvider,
                  closeOnBlur: true)
        {
            this.viewModel = viewModel;
            this.nativeBrowser = nativeBrowser;
            InitializeComponent();
            DataContext = viewModel;

            Loaded += OverlayWindow_Loaded;

            Show();
            Activate();

            if (GetMouseXPercent() > 0.5)
            {
                MoveWhenOpeningPreview = true;
                SetWindowPositionPercent(0.66 - GetWidthPercent(), 0.5 - (GetHeightPercent() / 2));
            }
            else
            {
                SetWindowPositionPercent(0.34, 0.5 - (GetHeightPercent() / 2));
            }

            if (viewModel.IsError)
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(1500);
                    Close();
                });
            }

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PriceViewModel.Results))
            {
                viewModel.Results.CollectionChanged += async (_, __) =>
                {
                    await Task.Delay(1000);
                    CheckLoadNewData();
                };
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
            CheckLoadNewData();
        }

        private void CheckLoadNewData()
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
            if (viewModel.PreviewItem == null && MoveWhenOpeningPreview)
            {
                MoveX(-260);
            }
            viewModel.Preview((PriceItem)ItemList.SelectedItem);
            EnsureBounds();
        }
    }
}
