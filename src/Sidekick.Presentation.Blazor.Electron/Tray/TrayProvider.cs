using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Application.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Localization.Tray;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<TrayProvider> logger;
        private readonly IViewLocator viewLocator;
        private readonly IMediator mediator;
        private readonly IKeyboardProvider keyboardProvider;

        public TrayProvider(
            IWebHostEnvironment webHostEnvironment,
            ILogger<TrayProvider> logger,
            IViewLocator viewLocator,
            IMediator mediator,
            IKeyboardProvider keyboardProvider
        )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.viewLocator = viewLocator;
            this.mediator = mediator;
            this.keyboardProvider = keyboardProvider;
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
                        Label = "Init",
                        Click = async () => {
                            await mediator.Send(new SaveSettingsCommand(new SidekickSettings()
                            {
                                LeagueId = "Ritual",
                                Language_Parser = "en",
                                Language_UI = "en",
                            }));

                            await mediator.Send(new InitializeCommand(true));
                        }
                    },

                    new MenuItem
                    {
                        Label = "Test Key",
                        Click = async () => {
                            keyboardProvider.Initialize();
                            await Task.Delay(5000);
                            await keyboardProvider.PressKey("a", "b", "c", "d", "e");
                        }
                    },

                    new MenuItem
                    {
                        Label = "About",
                        Click = () => { AboutView(); }
                    },

                    new MenuItem
                    {
                        Label = "Settings",
                        Click = () => { viewLocator.Open(View.Settings); }
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
