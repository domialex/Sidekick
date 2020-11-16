using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MediatR;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    public partial class PriceView : BaseOverlay
    {
        private readonly PriceViewModel viewModel;
        private readonly IMediator mediator;

        public PriceView(
            IServiceProvider serviceProvider,
            PriceViewModel viewModel,
            IMediator mediator)
            : base(Domain.Views.View.Price, serviceProvider)
        {

            this.viewModel = viewModel;
            this.mediator = mediator;
            InitializeComponent();
            DataContext = viewModel;

            Loaded += OverlayWindow_Loaded;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override async Task Open(params object[] args)
        {
            await base.Open(args);

            if (GetMouseXPercent() > 0.5)
            {
                SetLeftPercent(0.654, LocationSource.End);
            }
            else
            {
                SetLeftPercent(0.346, LocationSource.Begin);
            }
            SetTopPercent(50, LocationSource.Center);

            await viewModel.Initialize((Item)args[0]);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PriceViewModel.Results))
            {
                var scrollViewer = ItemList.GetChildOfType<ScrollViewer>();
                scrollViewer?.ScrollToTop();

                Title = viewModel.Item.NameLine ?? "";

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
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToTop();
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            CheckLoadNewData();
        }

        private void CheckLoadNewData()
        {
            var scrollViewer = ItemList.GetChildOfType<ScrollViewer>();

            // Load next results when scrollviewer is at the bottom
            // Query next page when reaching more than 99% of the scrollable content.
            if (scrollViewer != null && scrollViewer.ScrollableHeight > 0 && (scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight) > 0.99d)
            {
                _ = viewModel.LoadMoreData();
            }
        }

        private async void OpenLink(object sender, RequestNavigateEventArgs e)
        {
            await mediator.Send(new OpenBrowserCommand(e.Uri));
            e.Handled = true;
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.Preview((PriceItem)ItemList.SelectedItem);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = viewModel.LoadMoreData();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _ = viewModel.UpdateQuery();
        }
    }
}
