using System;
using System.Collections.Generic;
using ElectronNET.API.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;
using Sidekick.Localization.Tray;

namespace Sidekick.Presentation.Blazor.Electron.Tray
{
    public class TrayProvider
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<TrayProvider> logger;
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IMediator mediator;
        private readonly TrayResources resources;

        public TrayProvider(IWebHostEnvironment webHostEnvironment,
                            ILogger<TrayProvider> logger,
                            IViewLocator viewLocator,
                            IClipboardProvider clipboardProvider,
                            IMediator mediator,
                            TrayResources resources)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
            this.resources = resources;
        }

        public void Initialize()
        {
            try
            {
                var menuItems = new List<MenuItem>()
                {
                    new ()
                    {
                        Label = resources.Title + " - " + GetType().Assembly.GetName().Version.ToString(),
                        Type = MenuType.normal,
                        Icon = $"{webHostEnvironment.ContentRootPath}Assets/16x16.png",
                        Enabled = false,
                    },

                    new () { Type = MenuType.separator },

                    new ()
                    {
                        Label = resources.Cheatsheets,
                        Click = () => { viewLocator.Open(View.League); }
                    },

                    new ()
                    {
                        Label = resources.About,
                        Click = () => { viewLocator.Open(View.About); }
                    },

                    new ()
                    {
                        Label = resources.Settings,
                        Click = () => { viewLocator.Open(View.Settings); }
                    },

                    new () { Type = MenuType.separator },

                    new ()
                    {
                        Label = resources.Exit,
                        Click = () => ElectronNET.API.Electron.App.Quit()
                    }
                };

                if (webHostEnvironment.IsDevelopment())
                {
                    menuItems.InsertRange(0, GetDevelopmentMenu());
                }

                ElectronNET.API.Electron.Tray.Show($"{webHostEnvironment.ContentRootPath}Assets/icon.png", menuItems.ToArray());
                ElectronNET.API.Electron.Tray.OnDoubleClick += (_, _) => viewLocator.Open(View.Settings);
                ElectronNET.API.Electron.Tray.SetToolTip(resources.Title);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception while initializing the tray.");
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
                            Label = "Price check from clipboard",
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
                                var itemText = await clipboardProvider.GetText();
                                await viewLocator.Open(View.Map, itemText);
                            }
                        }
                    }
                },

                new () { Type = MenuType.separator },
            };
        }
    }
}
