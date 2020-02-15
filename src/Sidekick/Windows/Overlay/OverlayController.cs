using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Trades;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Natives;
using Sidekick.Localization;
using Application = System.Windows.Application;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.Overlay
{
    public class OverlayController
    {
        private readonly ITradeClient tradeClient;
        private readonly INativeProcess nativeProcess;
        private readonly OverlayWindow overlayWindow;

        private static readonly int WINDOW_WIDTH = 480;
        private static readonly int WINDOW_HEIGHT = 320;
        private static readonly int WINDOW_PADDING = 5;

        public OverlayController(IUILanguageProvider uiLanguageProvider, IPoePriceInfoClient poePriceInfoClient, ITradeClient tradeClient, INativeProcess nativeProcess)
        {
            this.tradeClient = tradeClient;
            this.nativeProcess = nativeProcess;

            overlayWindow = new OverlayWindow(WINDOW_WIDTH, WINDOW_HEIGHT, uiLanguageProvider.Current.Name, poePriceInfoClient);
            overlayWindow.MouseDown += Window_OnHandleMouseDrag;
            overlayWindow.ItemScrollReachedEnd += Window_ItemScrollReachedEnd;
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
            var scale = 96f / nativeProcess.ActiveWindowDpi;
            var xScaled = (int)(Cursor.Position.X * scale);
            var yScaled = (int)(Cursor.Position.Y * scale);

            EnsureBounds(xScaled, yScaled, scale);
            Show();
        }

        /// <summary>
        /// Ensures that the window stays within width and height of the display.
        /// </summary>
        private void EnsureBounds(int desiredX, int desiredY, float scale)
        {
            var screenRect = Screen.FromPoint(Cursor.Position).Bounds;
            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = desiredX + (desiredX < xMidScaled ? WINDOW_PADDING : -WINDOW_WIDTH - WINDOW_PADDING);
            var positionY = desiredY + (desiredY < yMidScaled ? WINDOW_PADDING : -WINDOW_HEIGHT - WINDOW_PADDING);

            overlayWindow.SetWindowPosition(positionX, positionY);
        }

        public Point GetOverlayPosition()
        {
            Debug.Assert(Application.Current.Dispatcher != null, "Application.Current.Dispatcher != null");
            return Application.Current.Dispatcher.Invoke(() => new Point(overlayWindow.Left, overlayWindow.Top));
        }

        public Size GetOverlaySize()
        {
            return new Size(overlayWindow.ActualWidth, overlayWindow.ActualHeight);
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
    }
}
