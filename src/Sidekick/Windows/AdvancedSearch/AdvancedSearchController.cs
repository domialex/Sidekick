using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.UI.Views;
using Sidekick.Windows.Prices;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Sidekick.Windows.AdvancedSearch
{
    public class AdvancedSearchController
    {
        private readonly INativeClipboard clipboard;
        private readonly ILogger logger;
        private readonly IItemParser itemParser;
        private readonly ITradeClient tradeClient;
        private readonly INativeProcess nativeProcess;
        private readonly INativeCursor nativeCursor;
        private readonly IViewLocator viewLocator;
        private readonly AdvancedSearchView view;
        private readonly SidekickSettings settings;

        public AdvancedSearchController(INativeClipboard clipboard, IKeybindEvents events, ILogger logger, IItemParser itemParser, ITradeClient tradeClient, IServiceProvider serviceProvider, INativeProcess nativeProcess, SidekickSettings settings, INativeCursor nativeCursor,
            IViewLocator viewLocator)
        {
            this.clipboard = clipboard;
            this.logger = logger;
            this.itemParser = itemParser;
            this.tradeClient = tradeClient;
            this.nativeProcess = nativeProcess;
            this.settings = settings;
            this.nativeCursor = nativeCursor;
            this.viewLocator = viewLocator;
            events.OnCloseWindow += OnCloseWindow;
            events.OnAdvancedSearch += OnAdvancedSearch;
            view = new AdvancedSearchView(this);
        }

        public bool IsDisplayed => view.Visibility == Visibility.Visible;
        public void Show() => view.ShowWindow();
        public void Hide() => view.HideWindowAndClearData();

        private Task<bool> OnCloseWindow()
        {
            if (IsDisplayed)
            {
                Hide();
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<bool> CheckItemPrice(Business.Parsers.Models.Item item)
        {
            view.HideWindowAndClearData();

            await clipboard.SetText(item.ItemText);
            viewLocator.Open<PriceView>();
            return true;
        }

        private async Task<bool> OnAdvancedSearch()
        {
            logger.Log("Hotkey for advanced search triggered");

            var text = await clipboard.Copy();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var item = await itemParser.ParseItem(text, true);

                if (item != null)
                {
                    view.PopulateGrid(item);
                    Open();
                    return true;
                }
            }

            view.HideWindowAndClearData();
            return false;
        }

        public void Open()
        {
            var scale = 96f / nativeProcess.ActiveWindowDpi;
            var cursorPosition = nativeCursor.GetCursorPosition();
            var xScaled = (int)(cursorPosition.X * scale);
            var yScaled = (int)(cursorPosition.Y * scale);

            EnsureBounds(xScaled, yScaled, scale);
            Show();
        }

        private void EnsureBounds(int desiredX, int desiredY, float scale)
        {
            var screenRect = nativeProcess.GetScreenDimensions();

            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = desiredX + (desiredX < xMidScaled ? view.Padding.Left : -view.Width - view.Padding.Left);
            var positionY = desiredY + (desiredY < yMidScaled ? view.Padding.Top : -view.Height - view.Padding.Top);

            view.SetWindowPosition((int)positionX, (int)positionY);
        }

        private void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                view.DragMove();
            }
        }

        public Point GetOverlayPosition()
        {
            Debug.Assert(Application.Current.Dispatcher != null, "Application.Current.Dispatcher != null");
            return Application.Current.Dispatcher.Invoke(() => new Point(view.Left, view.Top));
        }

        public Size GetOverlaySize()
        {
            return new Size(view.ActualWidth, view.ActualHeight);
        }

        private Task MouseClicked(int x, int y)
        {
            if (!IsDisplayed || !settings.CloseOverlayWithMouse) return Task.CompletedTask;

            var overlayPos = GetOverlayPosition();
            var overlaySize = GetOverlaySize();

            if (x < overlayPos.X || x > overlayPos.X + overlaySize.Width
                                 || y < overlayPos.Y || y > overlayPos.Y + overlaySize.Height)
            {
                Hide();
            }

            return Task.CompletedTask;
        }
    }
}
