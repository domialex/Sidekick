using Sidekick.Helpers;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI.Models;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Sidekick.Windows.Overlay
{
    public static class OverlayController
    {
        private static OverlayWindow _overlayWindow;
        private static int WINDOW_WIDTH = 480;
        private static int WINDOW_HEIGHT = 320;
        private static int PADDING = 5;

        public static void Initialize()
        {
            _overlayWindow = new OverlayWindow(WINDOW_WIDTH, WINDOW_HEIGHT);
        }

        public static bool IsDisplayed => _overlayWindow.IsDisplayed;
        public static void SetQueryResult(QueryResult<ListingResult> queryResult) => _overlayWindow.SetQueryResult(queryResult);

        public static void Show() => _overlayWindow.ShowWindow();

        public static void Hide() => _overlayWindow.HideWindowAndClearData();

        public static void SetPosition(int x, int y)
        {
            if (_overlayWindow == null)
                Initialize();

            // Ensure the window stays inside the screen but still appears on the mouse.
            var scale = 96f / ProcessHelper.GetActiveWindowDpi();
            var screenRect = Screen.FromPoint(Cursor.Position).Bounds;

            var xScaled = (int)(x * scale);
            var yScaled = (int)(y * scale);
            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = xScaled + (xScaled < xMidScaled ? PADDING : -WINDOW_WIDTH - PADDING);
            var positionY = yScaled + (yScaled < yMidScaled ? PADDING : -WINDOW_HEIGHT - PADDING);

            _overlayWindow.SetWindowPosition(positionX, positionY);
        }

        public static void Dispose()
        {
            _overlayWindow?.Close();
        }
    }
}
