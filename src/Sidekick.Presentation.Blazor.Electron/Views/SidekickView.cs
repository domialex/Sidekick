using System;
using ElectronNET.API;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    internal class SidekickView
    {
        public SidekickView(ViewLocator locator, View view, BrowserWindow browser)
        {
            Locator = locator;
            View = view;
            Browser = browser;

            Browser.OnReadyToShow += Browser_OnReadyToShow;
            Browser.OnResize += Browser_OnResize;
            Browser.OnClosed += Browser_OnClosed;
        }

        private void Browser_OnReadyToShow()
        {
            Browser.Show();
        }

        private void Browser_OnResize()
        {
            _ = Locator.debouncer.Debounce($"{nameof(SidekickView)}_{nameof(Browser_OnResize)}", async () =>
            {
                try
                {
                    var bounds = await Browser.GetBoundsAsync();
                    await Locator.viewPreferenceRepository.SaveSize(View, bounds.Width, bounds.Height);
                }
                catch (Exception e)
                {
                    Locator.logger.LogError("Failed to save the view size.", e);
                }
            });
        }

        private void Browser_OnClosed()
        {
            Locator.Views.Remove(this);
            Browser.OnReadyToShow -= Browser_OnReadyToShow;
            Browser.OnResize -= Browser_OnResize;
            Browser.OnClosed -= Browser_OnClosed;
        }

        public BrowserWindow Browser { get; }
        public ViewLocator Locator { get; }
        public View View { get; }
    }
}
