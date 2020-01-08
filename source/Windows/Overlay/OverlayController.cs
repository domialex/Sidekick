using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI.Models;
using System;
using System.Windows;

namespace Sidekick.Windows.Overlay
{
    public static class OverlayController
    {
        private static OverlayWindow _overlayWindow;
        private static int WINDOW_WIDTH = 480;
        private static int WINDOW_HEIGHT = 320;

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
            var screen = SystemParameters.WorkArea;
            var scale = screen.Width / ProcessHelper.GetActiveWindowWidth();
            var xScaled = (int)Math.Floor(x * scale);
            var yScaled = (int)Math.Floor(y * scale);

            var padding = 5;
            var positionX = xScaled + (xScaled < screen.Width / 2 ? padding : -WINDOW_WIDTH - padding);
            var positionY = yScaled + (yScaled < screen.Height / 2 ? padding : -WINDOW_HEIGHT - padding);


            _overlayWindow.SetWindowPosition(positionX, positionY);
        }

        public static void Dispose()
        {
            _overlayWindow?.Close();
        }
    }
}
