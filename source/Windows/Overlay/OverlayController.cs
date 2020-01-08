using Sidekick.Helpers.POENinjaAPI.Models;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Collections.Generic;
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
        public static void SetQueryResult(QueryResult<ListingResult> queryResult, PoeNinjaItem poeNinjaItem = null) => _overlayWindow.SetQueryResult(queryResult, poeNinjaItem);

        public static void Show() => _overlayWindow.ShowWindow();

        public static void Hide() => _overlayWindow.HideWindowAndClearData();

        public static void SetPosition(int x, int y)
        {
            if (_overlayWindow == null)
                Initialize();

            // Ensure the window stays inside the screen but still appears on the mouse.
            var screen = SystemParameters.WorkArea;
            var padding = 5;
            var positionX = x + (x < screen.Width / 2 ? padding : -WINDOW_WIDTH - padding);
            var positionY = y + (y < screen.Height / 2 ? padding : -WINDOW_HEIGHT - padding);

            _overlayWindow.SetWindowPosition(positionX, positionY);
        }

        public static void Dispose()
        {
            _overlayWindow?.Close();
        }
    }
}
