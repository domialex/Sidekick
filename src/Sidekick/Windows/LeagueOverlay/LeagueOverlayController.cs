using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.LeagueOverlay
{
    public static class LeagueOverlayController
    {
        private static LeagueOverlayView overlayWindow;
        private static int WindowPadding = 5;

        public static bool IsDisplayed => overlayWindow.IsDisplayed;
        public static void Show() => overlayWindow.ShowWindow();
        public static void Hide() => overlayWindow.HideWindow();

        public static void Initialize()
        {
            overlayWindow = new LeagueOverlayView();
            overlayWindow.MouseDown += Window_OnHandleMouseDrag;
        }

        public static void Open()
        {
            if(overlayWindow == null)
            {
                Initialize();
            }

            var scale = 96f / Legacy.NativeProcess.ActiveWindowDpi;
            var xScaled = (int)(Cursor.Position.X * scale);
            var yScaled = (int)(Cursor.Position.Y * scale);

            EnsureBounds(xScaled, yScaled, scale);
            Show();
        }

        private static void EnsureBounds(int desiredX, int desiredY, float scale)
        {
            var screenRect = Screen.FromPoint(Cursor.Position).Bounds;
            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = desiredX + (desiredX < xMidScaled ? WindowPadding : -overlayWindow.GetWidth() - WindowPadding);
            var positionY = desiredY + (desiredY < yMidScaled ? WindowPadding : -overlayWindow.GetHeight() - WindowPadding);

            overlayWindow.SetWindowPosition(positionX, positionY);
        }

        private static void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                overlayWindow.DragMove();
        }
    }
}
