using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Cache;
using Sidekick.Common.Settings;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        internal readonly ICacheProvider cacheProvider;
        internal readonly ILogger<ViewLocator> logger;
        internal readonly ISettings settings;
        internal readonly ElectronCookieProtection electronCookieProtection;
        internal readonly IHostEnvironment hostEnvironment;

        public ViewLocator(ICacheProvider cacheProvider,
                           ILogger<ViewLocator> logger,
                           ISettings settings,
                           ElectronCookieProtection electronCookieProtection,
                           IHostEnvironment hostEnvironment)
        {
            this.cacheProvider = cacheProvider;
            this.logger = logger;
            this.settings = settings;
            this.electronCookieProtection = electronCookieProtection;
            this.hostEnvironment = hostEnvironment;

            ElectronNET.API.Electron.IpcMain.OnSync("close", (viewName) =>
            {
                logger.LogError("Force closing the Blazor view {viewName}", viewName);
                if (!string.IsNullOrEmpty(viewName?.ToString()))
                {
                    Close(viewName.ToString());
                }
                else
                {
                    CloseAll();
                }

                return null;
            });
        }

        private bool FirstView = true;

        internal List<InternalViewInstance> Views { get; set; } = new List<InternalViewInstance>();

        private async Task<BrowserWindow> CreateBrowser(string path)
        {
            var window = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 768,
                Height = 600,
                AcceptFirstMouse = true,
                AlwaysOnTop = false,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                HasShadow = true,
                Maximizable = true,
                Minimizable = true,
                MinHeight = 600,
                MinWidth = 768,
                Resizable = true,
                Show = false,
                Transparent = true,
                DarkTheme = true,
                EnableLargerThanScreen = false,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = true,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}{path}");

            window.WebContents.OnCrashed += (killed) =>
            {
                logger.LogWarning("The view has crashed. Attempting to close the window.");
                window.Close();
            };

            window.OnUnresponsive += () =>
            {
                logger.LogWarning("The view has become unresponsive. Attempting to close the window.");
                window.Close();
            };

            await window.WebContents.Session.Cookies.SetAsync(electronCookieProtection.Cookie);

            // Make sure the title is always Sidekick. For keybind management we are watching for this value.
            window.SetTitle("Sidekick");
            window.SetVisibleOnAllWorkspaces(true);

            // Remove menu to disable the default hotkeys such as Ctrl+W and Ctrl+R.
            if (hostEnvironment.IsProduction())
            {
                window.RemoveMenu();
            }

            if (FirstView)
            {
                FirstView = false;
                await window.WebContents.Session.ClearCacheAsync();
            }

            return window;
        }

        public async Task Open(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            var newView = new InternalViewInstance(await CreateBrowser(url), this, url);

            foreach (var view in Views.Where(x => x.Key == newView.Key))
            {
                view.Browser.Close();
            }

            Views.Add(newView);
        }

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            foreach (var instance in Views)
            {
                instance.Browser.Close();
            }

            Views.Clear();
        }

        public void Close(string viewName)
        {
            foreach (var instance in Views.Where(x => x.Key == viewName))
            {
                instance.Browser.Close();
            }

            Views.RemoveAll(x => x.Key == viewName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CloseAll();
        }

        public void CloseAllOverlays()
        {
            throw new NotImplementedException();
        }

        public bool IsOverlayOpened()
        {
            throw new NotImplementedException();
        }
    }
}
