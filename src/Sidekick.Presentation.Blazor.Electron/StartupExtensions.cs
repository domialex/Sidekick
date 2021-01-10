using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Blazor.Electron.Tray;
using Sidekick.Presentation.Blazor.Electron.Views;

namespace Sidekick.Presentation.Blazor.Electron
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazorElectron(this IServiceCollection services)
        {
            services.AddSingleton<TrayProvider>();
            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }

        public static void UseSidekickPresentationBlazorElectron(this IApplicationBuilder _, IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            if (HybridSupport.IsElectronActive)
            {
                var trayProvider = serviceProvider.GetService<TrayProvider>();
                Task.Run(trayProvider.Initialize);

                ElectronBootstrap(webHostEnvironment);
            }
        }

        private static async void ElectronBootstrap(IWebHostEnvironment webHostEnvironment)
        {
            ElectronNET.API.Electron.WindowManager.IsQuitOnWindowAllClosed = true;

            var browserWindow = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Frame = false,
                Show = false,
                Transparent = true,
                WebPreferences = new WebPreferences()
                {
                    NodeIntegration = false,
                }
            });

            if (webHostEnvironment.IsDevelopment())
            {
                browserWindow.WebContents.OpenDevTools();
            }

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Sidekick");

            ElectronNET.API.Electron.IpcMain.On("ping", (args) =>
            {
                foreach (var window in ElectronNET.API.Electron.WindowManager.BrowserWindows)
                {
                    ElectronNET.API.Electron.IpcMain.Send(window, "pong");
                }
            });
        }
    }
}
