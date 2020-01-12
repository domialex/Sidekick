using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POEPriceInfoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.PricePrediction
{
    public static class PricePredictionViewController
    {
        private static PricePredictionView PriceView;

        private const int WindowWidth = 480;
        private const int WindowHeight = 320;
        private const int WindowPadding = 5;

        public static bool IsDisplayed => PriceView.IsDisplayed;
        public static void SetResult(PriceInfo info) => PriceView.SetResult(info);
        public static void Show() => PriceView.ShowWindow();
        public static void Hide() => PriceView.HideWindowAndClearData();

        public static void Initialize()
        {
            PriceView = new PricePredictionView(WindowWidth, WindowHeight);
            PriceView.MouseDown += Window_OnHandleMouseDrag;
        }

        public static void Dispose()
        {
            PriceView?.Close();
        }

        public static void Open()
        {
            if(PriceView == null)
            {
                Initialize();
            }

            var scale = 96f / ProcessHelper.GetActiveWindowDpi();
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

            var positionX = desiredX + (desiredX < xMidScaled ? WindowPadding : -WindowWidth - WindowPadding);
            var positionY = desiredY + (desiredY < yMidScaled ? WindowPadding : -WindowHeight - WindowPadding);

            PriceView.SetWindowPosition(positionX, positionY);
        }

        private static void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                PriceView.DragMove();
        }
    }
}
