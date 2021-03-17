using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewInstance : IViewInstance
    {
        private CancellationTokenSource WindowResizeCancellationTokenSource { get; }

        internal ViewLocator Locator { get; }
        internal View View { get; }
        internal BrowserWindow Browser { get; }

        internal ViewInstance(ViewLocator locator, View view, BrowserWindow browser)
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

            if (View == View.Price && Locator.settings.Price_CloseWithMouse)
            {
                Browser.Close();
            }
        }

        private void Browser_OnReadyToShow()
        {
            if (ViewLocator.IsOverlay(View))
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
            _ = Locator.debouncer.Debounce($"{nameof(ViewInstance)}_{nameof(Browser_OnResize)}", async () =>
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

        private readonly List<View> MinimizableViews = new()
        {
            View.About,
            View.League,
            View.Settings,
            View.Setup,
        };
        public bool Minimizable => MinimizableViews.Contains(View);

        public Task Minimize()
        {
            Browser.Minimize();
            return Task.CompletedTask;
        }

        private readonly List<View> MaximizableViews = new()
        {
            View.About,
            View.League,
            View.Price,
            View.Settings,
        };
        public bool Maximizable => MaximizableViews.Contains(View);

        public async Task Maximize()
        {
            if (!await Browser.IsMaximizedAsync())
            {
                Browser.Maximize();
            }
            else
            {
                var preferences = await Locator.viewPreferenceRepository.Get(View);
                if (preferences != null)
                {
                    Browser.SetSize(preferences.Width, preferences.Height);
                }
                else
                {
                    var (width, height) = ViewLocator.GetSize(View);
                    Browser.SetSize(width, height);
                }
                Browser.Center();
            }
        }

        public Task Close()
        {
            Browser.Close();
            return Task.CompletedTask;
        }
    }
}
