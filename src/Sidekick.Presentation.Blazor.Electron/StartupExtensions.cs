using System;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

        public static void UseSidekickPresentationBlazorElectron(this IApplicationBuilder _, IServiceProvider serviceProvider)
        {
            if (HybridSupport.IsElectronActive)
            {
                var trayProvider = serviceProvider.GetService<TrayProvider>();
                Task.Run(trayProvider.Initialize);

                ElectronBootstrap();
            }

            var viewLocator = serviceProvider.GetService<IViewLocator>();
            viewLocator.Open(View.About);
        }

        private static void ElectronBootstrap()
        {
            ElectronNET.API.Electron.WindowManager.IsQuitOnWindowAllClosed = false;
        }
    }
}
