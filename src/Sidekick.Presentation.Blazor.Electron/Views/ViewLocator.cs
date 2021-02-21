using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using Sidekick.Extensions;
using Sidekick.Presentation.Debounce;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        internal readonly IViewPreferenceRepository viewPreferenceRepository;
        internal readonly IDebouncer debouncer;
        internal readonly ILogger<ViewLocator> logger;
        internal readonly ISidekickSettings settings;

        public ViewLocator(IViewPreferenceRepository viewPreferenceRepository,
                           IDebouncer debouncer,
                           ILogger<ViewLocator> logger,
                           ISidekickSettings settings)
        {
            this.viewPreferenceRepository = viewPreferenceRepository;
            this.debouncer = debouncer;
            this.logger = logger;
            this.settings = settings;
        }

        private bool FirstView = true;

        internal List<SidekickView> Views { get; set; } = new List<SidekickView>();

        private static string GetUrl(View view, params object[] args)
        {
            var path = view switch
            {
                View.About => "/about",
                View.Settings => "/settings",
                View.Price => "/price",
                View.League => "/cheatsheets",
                View.Setup => "/setup",
                View.Initialization => "/initialization",
                View.Map => "/map",
                _ => null,
            };

            if (path == null)
            {
                return null;
            }

            foreach (var arg in args)
            {
                if (arg is string)
                {
                    path += $"/{arg}";
                }
                else
                {
                    path += $"/{JsonSerializer.Serialize(arg).EncodeBase64().EncodeUrl()}";
                }
            }

            return path;
        }

        private static (int Width, int Height) GetSize(View view)
        {
            return view switch
            {
                View.About => (800, 600),
                View.Settings => (800, 600),
                View.Price => (1200, 650),
                View.League => (800, 600),
                View.Setup => (600, 700),
                View.Initialization => (400, 215),
                View.Map => (500, 250),
                _ => (800, 600),
            };
        }

        private static async Task<BrowserWindow> CreateView(string path, int minWidth, int minHeight, ViewPreference preferences)
        {
            return await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = preferences?.Width ?? minWidth,
                Height = preferences?.Height ?? minHeight,
                AcceptFirstMouse = true,
                AlwaysOnTop = false,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                HasShadow = true,
                Maximizable = true,
                Minimizable = true,
                MinHeight = minHeight,
                MinWidth = minWidth,
                Resizable = true,
                Show = false,
                SkipTaskbar = false,
                Transparent = true,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}{path}");
        }

        private static async Task<BrowserWindow> CreateModal(string path, int minWidth, int minHeight, ViewPreference preferences)
        {
            return await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = preferences?.Width ?? minWidth,
                Height = preferences?.Height ?? minHeight,
                AcceptFirstMouse = true,
                AlwaysOnTop = true,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                Maximizable = false,
                Minimizable = false,
                MinHeight = minHeight,
                MinWidth = minWidth,
                Resizable = false,
                Show = false,
                SkipTaskbar = true,
                Transparent = true,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}{path}");
        }

        private static async Task<BrowserWindow> CreateOverlay(string path, int minWidth, int minHeight, ViewPreference preferences)
        {
            var window = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = preferences?.Width ?? minWidth,
                Height = preferences?.Height ?? minHeight,
                AcceptFirstMouse = true,
                AlwaysOnTop = true,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                Maximizable = false,
                Minimizable = false,
                MinHeight = minHeight,
                MinWidth = minWidth,
                Resizable = true,
                Show = false,
                SkipTaskbar = true,
                Transparent = true,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}{path}");

            return window;
        }

        public async Task Open(View view, params object[] args)
        {
            var url = GetUrl(view, args);
            var (width, height) = GetSize(view);

            if (IsOpened(view))
            {
                var openedView = Views.FirstOrDefault(x => x.View == view);
                if (openedView != null && openedView.Type == ViewType.View)
                {
                    openedView.Browser.LoadURL(url);
                    openedView.Browser.Focus();
                    return;
                }
                Close(view);
            }

            var preferences = await viewPreferenceRepository.Get(view);

            BrowserWindow browserWindow = null;
            var viewType = ViewType.View;
            switch (view)
            {
                case View.About:
                case View.League:
                case View.Settings:
                case View.Initialization:
                case View.Setup:
                    browserWindow = await CreateView(url, width, height, preferences);
                    viewType = ViewType.View;
                    break;
                case View.Price:
                case View.Map:
                    browserWindow = await CreateOverlay(url, width, height, preferences);
                    viewType = ViewType.Overlay;
                    break;
            }

            if (browserWindow == null)
            {
                return;
            }

            if (FirstView)
            {
                FirstView = false;
                await browserWindow.WebContents.Session.ClearCacheAsync();
            }

            // Make sure the title is always Sidekick. For keybind management we are watching for this value.
            browserWindow.SetTitle("Sidekick");

            var sidekickView = new SidekickView(this, view, viewType, browserWindow);

            Views.Add(sidekickView);
        }

        public bool IsOpened(View view) => Views.Any(x => x.View == view);

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            foreach (var instance in Views)
            {
                instance.Browser.Close();
            }

            Views.Clear();
        }

        public void Minimize(View view)
        {
            foreach (var instance in Views.Where(x => x.View == view))
            {
                instance.Browser.Minimize();
            }
        }

        public void Maximize(View view)
        {
            foreach (var instance in Views.Where(x => x.View == view))
            {
                instance.Browser.Maximize();
            }
        }

        public void Close(View view)
        {
            foreach (var instance in Views.Where(x => x.View == view))
            {
                instance.Browser.Close();
            }

            Views.RemoveAll(x => x.View == view);
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
    }
}
