using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POEPriceInfoAPI.Models;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.Prediction
{
    public static class PredictionController
    {
        private static PredictionView _predictionView;

        private const int WindowHeight = 450;
        private const int WindowWidth = 800;
        private const int WindowPadding = 5;

		public static bool IsDisplayed => _predictionView?.IsDisplayed ?? false;
        public static void SetPriceInfoResult(PriceInfoResult info) => _predictionView.SetPriceInfoResult(info);
        
        public static void Show() => _predictionView.ShowWindow();
        public static void Hide() => _predictionView.HideWindowAndClearData();

        public static void Initialize()
        {
            _predictionView = new PredictionView(WindowWidth, WindowHeight);
            _predictionView.MouseDown += Window_OnHandleMouseDrag;
        }

        public static void Dispose()
        {
            _predictionView?.Close();
        }

        public static void Open()
        {
            if (_predictionView == null)
                Initialize();

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

            _predictionView.SetWindowPosition(positionX, positionY);
        }

        private static void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                _predictionView.DragMove();
        }
    }
}
