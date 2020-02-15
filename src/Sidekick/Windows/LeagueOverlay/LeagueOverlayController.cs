using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Sidekick.Core.Natives;
using Sidekick.Localization;
using Cursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows.LeagueOverlay
{
    public class LeagueOverlayController
    {
        private readonly IKeybindEvents events;
        private readonly INativeProcess nativeProcess;
        private LeagueOverlayView overlayWindow;
        private int WindowPadding = 5;

        public LeagueOverlayController(IKeybindEvents events, INativeProcess nativeProcess, IUILanguageProvider languageProvider)
        {
            this.events = events;
            this.nativeProcess = nativeProcess;

            overlayWindow = new LeagueOverlayView(languageProvider);
            overlayWindow.MouseDown += Window_OnHandleMouseDrag;

            events.OnCloseWindow += OnCloseWindow;
            events.OnOpenLeagueOverview += OnOpenLeagueOverview;
        }



        public bool IsDisplayed => overlayWindow.IsDisplayed;
        public void Show() => overlayWindow.ShowWindow();
        public void Hide() => overlayWindow.HideWindow();

        public void Open()
        {
            var scale = 96f / nativeProcess.ActiveWindowDpi;

            var xScaled = Screen.PrimaryScreen.WorkingArea.Width / 4;
            var yScaled = Screen.PrimaryScreen.WorkingArea.Top;

            EnsureBounds(xScaled, yScaled, scale);
            Show();
        }

        private void EnsureBounds(int desiredX, int desiredY, float scale)
        {
            var screenRect = Screen.FromPoint(Cursor.Position).Bounds;
            var xMidScaled = (screenRect.X + (screenRect.Width / 2)) * scale;
            var yMidScaled = (screenRect.Y + (screenRect.Height / 2)) * scale;

            var positionX = desiredX + (desiredX < xMidScaled ? WindowPadding : -overlayWindow.GetWidth() - WindowPadding);
            var positionY = desiredY + (desiredY < yMidScaled ? WindowPadding : -overlayWindow.GetHeight() - WindowPadding);

            overlayWindow.SetWindowPosition(positionX, positionY);
        }

        private void Window_OnHandleMouseDrag(object sender, MouseButtonEventArgs e)
        {
            // automatically detects mouse up/down states
            if (e.ChangedButton == MouseButton.Left)
                overlayWindow.DragMove();
        }

        private Task<bool> OnOpenLeagueOverview()
        {
            Open();
            return Task.FromResult(true);
        }

        private Task<bool> OnCloseWindow()
        {
            var handled = false;
            if (IsDisplayed)
            {
                Hide();
                handled = true;
            }

            return Task.FromResult(handled);
        }
    }
}
