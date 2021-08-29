using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    internal class InternalViewInstance
    {
        internal ViewLocator Locator { get; }
        internal BrowserWindow Browser { get; }
        internal string Url { get; }
        internal string Key { get; }
        internal bool IsModal { get; set; }
        internal bool IsOverlay { get; set; }
        internal bool CloseOnBlur { get; set; }

        internal InternalViewInstance(BrowserWindow browser, ViewLocator locator, string url)
        {
            Locator = locator;
            Browser = browser;
            Url = url;
            Key = url.Split('/').FirstOrDefault(x => !string.IsNullOrEmpty(x));

            Browser.OnResize += Browser_OnResize;
            Browser.OnClosed += Browser_OnClosed;
            Browser.OnBlur += Browser_OnBlur;
        }

        private void Browser_OnBlur()
        {
            if (CloseOnBlur)
            {
                Browser.Close();
            }
        }

        private ulong resizeBounce = 0;
        private void Browser_OnResize()
        {
            Task.Run(async () =>
            {
                var currentBounce = ++resizeBounce;
                await Task.Delay(500);
                if (currentBounce == resizeBounce)
                {
                    if (!await Browser.IsMaximizedAsync())
                    {
                        var bounds = await Browser.GetBoundsAsync();
                        await Locator.cacheProvider.Set($"view_preference_{Key}", new ViewPreferences()
                        {
                            Width = bounds.Width,
                            Height = bounds.Height
                        });
                    }
                }
            });
        }

        private void Browser_OnClosed()
        {
            resizeBounce++;
            Browser.OnResize -= Browser_OnResize;
            Browser.OnClosed -= Browser_OnClosed;
            Browser.OnBlur -= Browser_OnBlur;
            Locator.Views.Remove(this);
        }
    }
}
