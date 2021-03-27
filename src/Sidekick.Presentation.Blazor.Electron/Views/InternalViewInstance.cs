using System;
using System.Threading;
using ElectronNET.API;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    internal class InternalViewInstance
    {
        private CancellationTokenSource WindowResizeCancellationTokenSource { get; }

        internal ViewLocator Locator { get; }
        internal View View { get; }
        internal BrowserWindow Browser { get; }

        internal InternalViewInstance(ViewLocator locator, View view, BrowserWindow browser)
        {
            WindowResizeCancellationTokenSource = new CancellationTokenSource();

            Locator = locator;
            View = view;
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

            if (View == View.Trade && Locator.settings.Price_CloseWithMouse)
            {
                Browser.Close();
            }
        }

        private void Browser_OnReadyToShow()
        {
            if (ViewLocator.IsOverlay(View) || ViewLocator.IsModal(View))
            {
                Browser.ShowInactive();
            }
            else
            {
                Browser.Show();
            }
        }

        private void Browser_OnResize()
        {
            _ = Locator.debouncer.Debounce($"{nameof(InternalViewInstance)}_{nameof(Browser_OnResize)}", async () =>
            {
                try
                {
                    if (!await Browser.IsMaximizedAsync())
                    {
                        var bounds = await Browser.GetBoundsAsync();
                        await Locator.viewPreferenceRepository.SaveSize(View, bounds.Width, bounds.Height);
                    }
                }
                catch (Exception e)
                {
                    Locator.logger.LogError("Failed to save the view size.", e);
                }
            }, WindowResizeCancellationTokenSource.Token, delay: 500);
        }

        private void Browser_OnClosed()
        {
            WindowResizeCancellationTokenSource.Cancel();
            Browser.OnReadyToShow -= Browser_OnReadyToShow;
            Browser.OnResize -= Browser_OnResize;
            Browser.OnClosed -= Browser_OnClosed;
            Browser.OnBlur -= Browser_OnBlur;
            Locator.Views.Remove(this);
        }
    }
}
