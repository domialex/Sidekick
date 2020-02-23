using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;

namespace Sidekick.Windows.Prices
{
    public class OverlayController : IDisposable
    {
        private readonly ITradeClient tradeClient;
        private readonly INativeClipboard clipboard;
        private readonly ILogger logger;
        private readonly IItemParser itemParser;
        private readonly IPoeNinjaCache poeNinjaCache;
        private readonly OverlayWindow overlayWindow;

        public OverlayController(
            ITradeClient tradeClient,
            IKeybindEvents events,
            INativeClipboard clipboard,
            ILogger logger,
            IItemParser itemParser,
            IPoeNinjaCache poeNinjaCache,
            IServiceProvider serviceProvider)
        {
            this.tradeClient = tradeClient;
            this.clipboard = clipboard;
            this.logger = logger;
            this.itemParser = itemParser;
            this.poeNinjaCache = poeNinjaCache;

            overlayWindow = serviceProvider.GetService<OverlayWindow>();
            overlayWindow.MouseDown += Window_OnHandleMouseDrag;
            overlayWindow.ItemScrollReachedEnd += Window_ItemScrollReachedEnd;

            events.OnCloseWindow += OnCloseWindow;
            events.OnPriceCheck += OnPriceCheck;
        }

        public bool IsDisplayed => overlayWindow.IsDisplayed;
        public void SetQueryResult(QueryResult<ListingResult> queryResult) => overlayWindow.SetQueryResult(queryResult);
        public void Show() => overlayWindow.ShowWindow();
        public void Hide() => overlayWindow.HideWindowAndClearData();

        private async void Window_ItemScrollReachedEnd(Business.Parsers.Models.Item item, int displayedItemsCount)
        {
            var page = (int)Math.Ceiling(displayedItemsCount / 10d);
            var queryResult = await tradeClient.GetListingsForSubsequentPages(item, page);
            if (queryResult.Result.Any())
            {
                overlayWindow.AppendQueryResult(queryResult);
            }
        }

        public void Dispose()
        {
            overlayWindow?.Close();
        }

        /// <summary>
        /// Opens the window at the current cursor position.
        /// Ensures that the window will be inside the current screen bounds.
        /// </summary>
        public void Open()
        {
            Show();
        }

        /// <summary>
        /// Handles dragging for the window when pressing any control that does NOT handle the event <see cref="RoutedEventArgs.Handled"/>.
        /// Does not apply clamping of the windows position.
        /// </summary>
        private void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                overlayWindow.DragMove();
        }

        private Task<bool> OnCloseWindow()
        {
            var handled = false;

            if (IsDisplayed)
            {
                Hide();
                handled = true;
            }

            return Task.FromResult(handled);
        }

        private async Task<bool> OnPriceCheck()
        {
            logger.Log("Hotkey for pricing item triggered.");

            var text = await clipboard.Copy();
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

            return false;
        }
    }
}
