using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Sidekick.Core.Natives;
using Sidekick.Localization;

namespace Sidekick.Windows.LeagueOverlay
{
    public class LeagueOverlayController : IDisposable
    {
        private readonly IKeybindEvents events;
        private readonly INativeProcess nativeProcess;
        private LeagueOverlayView overlayWindow;

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
            Show();
        }

        public void Dispose()
        {
            overlayWindow.MouseDown -= Window_OnHandleMouseDrag;
            events.OnCloseWindow -= OnCloseWindow;
            events.OnOpenLeagueOverview -= OnOpenLeagueOverview;
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
