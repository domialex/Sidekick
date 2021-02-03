using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;
using Sidekick.Extensions;
using Sidekick.Presentation.Debounce;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public readonly IViewPreferenceRepository viewPreferenceRepository;
        public readonly IDebouncer debouncer;
        public readonly ILogger<ViewLocator> logger;

        public ViewLocator(IWebHostEnvironment webHostEnvironment,
            IViewPreferenceRepository viewPreferenceRepository,
            IDebouncer debouncer,
            ILogger<ViewLocator> logger)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.viewPreferenceRepository = viewPreferenceRepository;
            this.debouncer = debouncer;
            this.logger = logger;
        }

        private bool FirstView = true;

        internal List<SidekickView> Views { get; set; } = new List<SidekickView>();

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
                // Icon = "/Assets/ExaltedOrb.ico",
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

        private static async Task<BrowserWindow> CreateOverlay(string path, int minWidth, int minHeight, ViewPreference preferences)
        {
            var window =  await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = preferences?.Width ?? minWidth,
                Height = preferences?.Height ?? minHeight,
                AcceptFirstMouse = true,
                AlwaysOnTop = true,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                HasShadow = false,
                // Icon = "Assets/ExaltedOrb.ico",
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
            if (IsOpened(view))
            {
                Close(view);
            }

            var preferences = await viewPreferenceRepository.Get(view);

            var pathArgs = new StringBuilder();
            foreach (var arg in args)
            {
                pathArgs.Append($"/{JsonSerializer.Serialize(arg).EncodeBase64().EncodeUrl()}");
            }

            var browserWindow = view switch
            {
                View.About => await CreateView($"/about{pathArgs}", 800, 600, preferences),
                View.Settings => await CreateView($"/settings{pathArgs}", 800, 600, preferences),
                View.Price => await CreateView($"/settings{pathArgs}", 800, 600, preferences),
                View.Setup => await CreateView($"/about{pathArgs}", 800, 600, preferences),
                View.Initialization => await CreateView($"/about{pathArgs}", 800, 600, preferences),
                View.Map => await CreateOverlay($"/map{pathArgs}", 500, 250, preferences),
                _ => null,
            };

            if (browserWindow == null)
            {
                return;
            }

            if (webHostEnvironment.IsDevelopment())
            {
                browserWindow.WebContents.OpenDevTools();
            }

            if (FirstView)
            {
                FirstView = false;
                await browserWindow.WebContents.Session.ClearCacheAsync();
            }

            browserWindow.SetTitle("Sidekick");

            var sidekickView = new SidekickView(this, view, browserWindow);

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
