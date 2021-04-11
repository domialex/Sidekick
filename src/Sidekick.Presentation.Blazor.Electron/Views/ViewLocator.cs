using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using Sidekick.Extensions;
using Sidekick.Presentation.Blazor.Debounce;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        internal readonly IViewPreferenceRepository viewPreferenceRepository;
        internal readonly IDebouncer debouncer;
        internal readonly ILogger<ViewLocator> logger;
        internal readonly ISidekickSettings settings;
        internal readonly ElectronCookieProtection electronCookieProtection;
        internal readonly IHostEnvironment hostEnvironment;

        public ViewLocator(IViewPreferenceRepository viewPreferenceRepository,
                           IDebouncer debouncer,
                           ILogger<ViewLocator> logger,
                           ISidekickSettings settings,
                           ElectronCookieProtection electronCookieProtection,
                           IHostEnvironment hostEnvironment)
        {
            this.viewPreferenceRepository = viewPreferenceRepository;
            this.debouncer = debouncer;
            this.logger = logger;
            this.settings = settings;
            this.electronCookieProtection = electronCookieProtection;
            this.hostEnvironment = hostEnvironment;
        }

        private bool FirstView = true;

        internal List<InternalViewInstance> Views { get; set; } = new List<InternalViewInstance>();

        private static string GetPath(View view, params object[] args)
        {
            var path = view switch
            {
                View.About => "/about",
                View.Error => "/error",
                View.Settings => "/settings",
                View.Trade => "/price",
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
                path += $"/{arg.ToString().EncodeBase64Url()}";
            }

            return path;
        }

        internal static (int Width, int Height) GetSize(View view)
        {
            return view switch
            {
                View.About => (800, 600),
                View.Settings => (800, 600),
                View.Trade => (768, 600),
                View.League => (800, 600),
                View.Setup => (600, 715),
                View.Initialization => (400, 260),
                View.Map => (400, 300),
                View.Error => (300, 200),
                _ => (800, 600),
            };
        }

        private static readonly List<View> ModalViews = new()
        {
            View.Initialization,
            View.Setup,
            View.Error,
        };
        internal static bool IsModal(View view) => ModalViews.Contains(view);

        private static readonly List<View> OverlayViews = new()
        {
            View.Map,
            View.Trade,
            View.Error,
        };
        internal static bool IsOverlay(View view) => OverlayViews.Contains(view);

        private async Task<BrowserWindow> CreateBrowser(View view, string path)
        {
            var preferences = await viewPreferenceRepository.Get(view);
            var (width, height) = GetSize(view);
            var isOverlay = IsOverlay(view);
            var isModal = IsModal(view);

            var window = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = preferences?.Width ?? width,
                Height = preferences?.Height ?? height,
                AcceptFirstMouse = true,
                AlwaysOnTop = isOverlay,
                Center = true,
                Frame = false,
                Fullscreenable = false,
                HasShadow = true,
                Maximizable = !isOverlay && !isModal,
                Minimizable = !isOverlay && !isModal,
                MinHeight = height,
                MinWidth = width,
                Resizable = !isModal,
                Show = false,
                SkipTaskbar = isOverlay,
                Transparent = true,
                DarkTheme = true,
                EnableLargerThanScreen = false,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}{path}");

            await window.WebContents.Session.Cookies.SetAsync(electronCookieProtection.Cookie);

            // Make sure the title is always Sidekick. For keybind management we are watching for this value.
            window.SetTitle("Sidekick");
            window.SetVisibleOnAllWorkspaces(true);

            // Remove menu to disable the default hotkeys such as Ctrl+W and Ctrl+R.
            if (hostEnvironment.IsProduction())
            {
                window.RemoveMenu();
            }

            if (isOverlay)
            {
                window.SetAlwaysOnTop(true, OnTopLevel.screenSaver);
            }

            if (FirstView)
            {
                FirstView = false;
                await window.WebContents.Session.ClearCacheAsync();
            }

            return window;
        }

        public async Task Open(View view, params object[] args)
        {
            var url = GetPath(view, args);

            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            if (IsOpened(view))
            {
                Close(view);
            }

            Views.Add(new InternalViewInstance(this, view, await CreateBrowser(view, url)));
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
