using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Sidekick.Presentation.Localization.Tray;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public TrayProvider(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public Task Initialize()
        {
            var menuItems = new List<MenuItem>
            {
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
