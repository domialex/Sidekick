using System;
using ElectronNET.API;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    internal class SidekickView
    {
        public SidekickView(ViewLocator locator, View view, ViewType type, BrowserWindow browser)
        {
            Locator = locator;
            View = view;
            Type = type;
            Browser = browser;

            Browser.OnReadyToShow += Browser_OnReadyToShow;
            Browser.OnResize += Browser_OnResize;
            Browser.OnClosed += Browser_OnClosed;
            Browser.OnBlur += Browser_OnBlur;
        }

        private void Browser_OnBlur()
        {
            if (View == View.Map && Locator.settings.Map_CloseWithMouse)
            {
                Browser.Close();
            }

            if (View == View.Price && Locator.settings.Price_CloseWithMouse)
            {
                Browser.Close();
            }
        }

        private void Browser_OnReadyToShow()
        {
            Browser.Show();
            Browser.Focus();
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
            Browser.OnBlur -= Browser_OnBlur;
        }

        public BrowserWindow Browser { get; }
        public ViewLocator Locator { get; }
        public View View { get; }
        public ViewType Type { get; }
    }
}
