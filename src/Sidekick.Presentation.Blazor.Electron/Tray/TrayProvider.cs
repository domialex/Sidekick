using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Localization.Tray;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IServiceProvider serviceProvider;
        private readonly IViewLocator viewLocator;

        public TrayProvider(IWebHostEnvironment webHostEnvironment, IServiceProvider serviceProvider, IViewLocator viewLocator)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.serviceProvider = serviceProvider;
            this.viewLocator = viewLocator;
        }

        public Task Initialize()
        {
            if (!HybridSupport.IsElectronActive)
            {
                return default;
            }

            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Label = "About",
                    Click = () => { viewLocator.Open(View.About); }
                },

                new MenuItem
                {
                    Label = TrayResources.Exit,
                    Click = () => ElectronNET.API.Electron.App.Quit()
                }
            };

            ElectronNET.API.Electron.Tray.Show($"{webHostEnvironment.ContentRootPath}Assets/ExaltedOrb.png", menuItems.ToArray());
            ElectronNET.API.Electron.Tray.SetToolTip(TrayResources.Title);

            return Task.CompletedTask;
        }
    }
}
