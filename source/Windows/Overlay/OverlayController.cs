using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.Overlay
{
    public static class OverlayController
    {
        private static OverlayWindow _overlayWindow;

        private static readonly int WINDOW_WIDTH = 480;
        private static readonly int WINDOW_HEIGHT = 320;
        private static readonly int WINDOW_PADDING = 5;

        public static bool IsDisplayed => _overlayWindow.IsDisplayed;
        public static void SetQueryResult(QueryResult<ListingResult> queryResult) => _overlayWindow.SetQueryResult(queryResult);
        public static void Show() => _overlayWindow.ShowWindow();
        public static void Hide() => _overlayWindow.HideWindowAndClearData();

        public static void Initialize()
        {
            _overlayWindow = new OverlayWindow(WINDOW_WIDTH, WINDOW_HEIGHT);
            _overlayWindow.MouseDown += Window_OnHandleMouseDrag;
            _overlayWindow.ItemScrollReachedEnd += Window_ItemScrollReachedEnd;
        }

        private static void Window_ItemScrollReachedEnd(Helpers.Item item, int displayedItemsCount)
        {
            var queryResult = Task.Run(() => TradeClient.GetListingsForSubsequentPages(item, (int)System.Math.Ceiling(displayedItemsCount / 10d))).Result;
            _overlayWindow.AppendQueryResult(queryResult);
        }

        public static void Dispose()
        {
            _overlayWindow?.Close();
        }

        /// <summary>
        /// Opens the window at the current cursor position.
        /// Ensures that the window will be inside the current screen bounds.
        /// </summary>
        public static void Open()
        {
            if (_overlayWindow == null)
                Initialize();

            var scale = 96f / ProcessHelper.GetActiveWindowDpi();
            var xScaled = (int)(Cursor.Position.X * scale);
            var yScaled = (int)(Cursor.Position.Y * scale);

            EnsureBounds(xScaled, yScaled, scale);
            Show();
        }

        /// <summary>
        /// Ensures that the window stays within width and height of the display.
        /// </summary>
        private static void EnsureBounds(int desiredX, int desiredY, float scale)
        {
            var screenRect = Screen.FromPoint(Cursor.Position).Bounds;
            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = desiredX + (desiredX < xMidScaled ? WINDOW_PADDING : -WINDOW_WIDTH - WINDOW_PADDING);
            var positionY = desiredY + (desiredY < yMidScaled ? WINDOW_PADDING : -WINDOW_HEIGHT - WINDOW_PADDING);

            _overlayWindow.SetWindowPosition(positionX, positionY);
        }

        /// <summary>
        /// Handles dragging for the window when pressing any control that does NOT handle the event <see cref="RoutedEventArgs.Handled"/>.
        /// Does not apply clamping of the windows position.
        /// </summary>
        private static void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                _overlayWindow.DragMove();
        }
    }
}
