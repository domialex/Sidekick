using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ViewLocator(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        private bool FirstView = true;

        private List<SidekickView> Views { get; set; } = new List<SidekickView>();

        public async Task Open(View view, params object[] args)
        {
            var browserWindow = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Frame = false,
                Show = false,
                Transparent = true,
                Fullscreenable = false,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            }, $"http://localhost:{BridgeSettings.WebPort}/{view}");

            if (webHostEnvironment.IsDevelopment())
            {
                browserWindow.WebContents.OpenDevTools();
            }

            if (FirstView)
            {
                FirstView = false;
                await browserWindow.WebContents.Session.ClearCacheAsync();
            }

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Sidekick");

            Views.Add(new SidekickView()
            {
                Browser = browserWindow,
                View = view,
            });
        }

        public bool IsOpened(View view) => Views.Any(x => x.View == view);

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            foreach (var instance in Views)
            {
                instance.Browser.Close();
                Views.Remove(instance);
            }
        }

        public void Close(View view)
        {
            foreach (var instance in Views.Where(x => x.View == view))
            {
                instance.Browser.Close();
                Views.Remove(instance);
            }
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
