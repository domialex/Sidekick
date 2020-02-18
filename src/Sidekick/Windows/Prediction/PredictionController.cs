using System;
using System.Windows.Forms;
using System.Windows.Input;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Core.Natives;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.Prediction
{
    public class PredictionController : IDisposable
    {
        private readonly INativeProcess nativeProcess;
        private static PredictionView _predictionView;

        private const int WindowHeight = 450;
        private const int WindowWidth = 800;
        private const int WindowPadding = 5;

        public bool IsDisplayed => _predictionView?.IsDisplayed ?? false;
        public void SetPriceInfoResult(PriceInfoResult info) => _predictionView.SetPriceInfoResult(info);

        public void Show() => _predictionView.ShowWindow();
        public void Hide() => _predictionView.HideWindowAndClearData();

        public PredictionController(INativeProcess nativeProcess)
        {
            this.nativeProcess = nativeProcess;
            _predictionView = new PredictionView(WindowWidth, WindowHeight);
            _predictionView.MouseDown += Window_OnHandleMouseDrag;
        }

        public void Dispose()
        {
            _predictionView?.Close();
        }

        public void Open()
        {
            var scale = 96f / nativeProcess.ActiveWindowDpi;
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
