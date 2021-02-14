using System;
using ElectronNET.API.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Localization.Tray;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Collections.Generic;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private IWebHostEnvironment webHostEnvironment { get; init; }
        private ILogger<TrayProvider> logger { get; init; }
        private IViewLocator viewLocator { get; init; }
        private IClipboardProvider clipboardProvider { get; init; }
        private IMediator mediator { get; init; }

        public TrayProvider(IWebHostEnvironment webHostEnvironment,
                            ILogger<TrayProvider> logger,
                            IViewLocator viewLocator,
                            IClipboardProvider clipboardProvider,
                            IMediator mediator)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
        }

        public void Initialize()
        {
            try
            {
                var menuItems = new List<MenuItem>()
                {
                    new ()
                    {
                        Label = "Cheatsheets",
                        Click = () => { viewLocator.Open(View.League); }
                    },

                    new ()
                    {
                        Label = "About",
                        Click = () => { viewLocator.Open(View.About); }
                    },

                    new ()
                    {
                        Label = "Settings",
                        Click = () => { viewLocator.Open(View.Settings); }
                    },

                    new () { Type = MenuType.separator },

                    new ()
                    {
                        Label = TrayResources.Exit,
                        Click = () => ElectronNET.API.Electron.App.Quit()
                    }
                };


                if (webHostEnvironment.IsDevelopment())
                {
                    menuItems.InsertRange(0, GetDevelopmentMenu());
                }

                ElectronNET.API.Electron.Tray.Show($"{webHostEnvironment.ContentRootPath}Assets/ExaltedOrb.png", menuItems.ToArray());
                ElectronNET.API.Electron.Tray.OnDoubleClick += (_, _) => viewLocator.Open(View.Settings);
                ElectronNET.API.Electron.Tray.SetToolTip(TrayResources.Title);
            }
            catch (Exception e)
            {
                logger.LogError("Exception while initializing the tray.", e);
            }
        }

        public List<MenuItem> GetDevelopmentMenu()
        {
            return new List<MenuItem>()
            {
                 new () {
                    Label = "Development",
                    Type = MenuType.submenu,
                    Submenu = new MenuItem[]
                    {
                        new () {
                            Label = "Parse item from clipboard",
                            Click = async () =>
                            {
                                var item = await mediator.Send(new ParseItemCommand(await clipboardProvider.GetText()));
                                if (item != null)
                                {
                                    await mediator.Send(new PriceCheckItemCommand(item));
                                }
                            }
                        },
                        new () {
                            Label = "Check map from clipboard",
                            Click = async () =>
                            {
                                var item = await mediator.Send(new ParseItemCommand(await clipboardProvider.GetText()));
                                if (item !=null && item.Properties.MapTier != 0)
                                {
                                    await viewLocator.Open(View.Map, item);
                                }
                            }
                        }
                    }
                },

                new () { Type = MenuType.separator },
            };
        }
    }
}
