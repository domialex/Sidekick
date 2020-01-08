using Sidekick.Helpers.POETradeAPI.Models;
using System.Windows;
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

            EnsureBounds(Cursor.Position.X, Cursor.Position.Y);
            Show();
        }

        /// <summary>
        /// Ensures that the window stays within width and height of the display.
        /// </summary>
        private static void EnsureBounds(int desiredX, int desiredY)
        {
            var screen = SystemParameters.WorkArea;
            int x = desiredX + (desiredX < screen.Width / 2 ? WINDOW_PADDING : -WINDOW_WIDTH - WINDOW_PADDING);
            int y = desiredY + (desiredY < screen.Height / 2 ? WINDOW_PADDING : -WINDOW_HEIGHT - WINDOW_PADDING);

            _overlayWindow.SetWindowPosition(x, y);
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
