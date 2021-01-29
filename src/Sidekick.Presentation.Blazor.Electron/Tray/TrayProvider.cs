using System;
using System.Collections.Generic;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Localization.Tray;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<TrayProvider> logger;
        private readonly IViewLocator viewLocator;

        public TrayProvider(
            IWebHostEnvironment webHostEnvironment,
            ILogger<TrayProvider> logger,
            IViewLocator viewLocator
        )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.viewLocator = viewLocator;
        }

        public void AboutView()
        {
            viewLocator.Open(View.About);
        }

        public void Initialize()
        {
            try
            {
                var menuItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Label = "About",
                        Click = () => { AboutView(); }
                    },

                    new MenuItem
                    {
                        Label = TrayResources.Exit,
                        Click = () => ElectronNET.API.Electron.App.Quit()
                    }
                };

                ElectronNET.API.Electron.Tray.Show($"{webHostEnvironment.ContentRootPath}Assets/ExaltedOrb.png", menuItems.ToArray());
                ElectronNET.API.Electron.Tray.SetToolTip(TrayResources.Title);
            }
            catch (Exception e)
            {
                logger.LogError("Exception while initializing the tray.", e);
            }
        }
    }
}
