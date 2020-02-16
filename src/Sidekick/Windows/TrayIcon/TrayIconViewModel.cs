using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using Sidekick.Localization;
using Sidekick.Localization.Tray;
using Sidekick.UI.Views;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Leagues;
using Sidekick.Windows.PriceCheck;
using Sidekick.Windows.Settings;

namespace Sidekick.Windows.TrayIcon
{
    public class TrayIconViewModel : IOnAfterInit, IDisposable
    {
        private readonly App application;
        private readonly SidekickSettings settings;
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly IViewLocator viewLocator;
        private readonly ApplicationLogsController applicationLogsController;

        public TrayIconViewModel(
            App application,
            SidekickSettings settings,
            IUILanguageProvider uiLanguageProvider,
            IViewLocator viewLocator,
            ApplicationLogsController applicationLogsController)
        {
            this.application = application;
            this.settings = settings;
            this.uiLanguageProvider = uiLanguageProvider;
            this.viewLocator = viewLocator;
            this.applicationLogsController = applicationLogsController;
        }

        private TaskbarIcon TrayIcon { get; set; }

        public ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => applicationLogsController.Show());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());

        public ContextMenu ContextMenu { get; set; }

        public Task OnAfterInit()
        {
            if (TrayIcon == null)
            {
                TrayIcon = (TaskbarIcon)application.FindResource("TrayIcon");
                TrayIcon.DataContext = this;

                uiLanguageProvider.UILanguageChanged += InitContextMenu;
                InitContextMenu();

                TrayIcon.ShowBalloonTip(
                    TrayResources.Notification_Title,
                    string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString()),
                    TrayIcon.Icon,
                    largeIcon: true);
            }

            return Task.CompletedTask;
        }

        private void InitContextMenu()
        {
            var cultureInfo = new CultureInfo(uiLanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            if (TrayIcon.ContextMenu == null)
            {
                TrayIcon.ContextMenu = new ContextMenu();
            }

            TrayIcon.ContextMenu.Items.Clear();

#if DEBUG
            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "DEBUG - Price check",
                Command = new RelayCommand(async (_) =>
                {
                    var item = await Legacy.ItemParser.ParseItem(@"Rarity: Unique
Blood of the Karui
Sanctified Life Flask
--------
Quality: +20% (augmented)
Recovers 3504 (augmented) Life over 2.60 (augmented) Seconds
Consumes 15 of 30 Charges on use
Currently has 30 Charges
--------
Requirements:
Level: 50
--------
Item Level: 75
--------
100% increased Life Recovered
15% increased Recovery rate
Recover Full Life at the end of the Flask Effect
--------
""Kaom fought and killed for his people.
Kaom bled for his people.
And so the people gave, the people bled,
So their King might go on.""
- Lavianga, Advisor to Kaom
--------
Right click to drink.Can only hold charges while in belt.Refills as you kill monsters.
");

                    if (item != null)
                    {
                        OverlayController.Open();

                        var queryResult = await Legacy.TradeClient.GetListings(item);
                        if (queryResult != null)
                        {
                            var poeNinjaItem = Legacy.PoeNinjaCache.GetItem(item);
                            if (poeNinjaItem != null)
                            {
                                queryResult.PoeNinjaItem = poeNinjaItem;
                                queryResult.LastRefreshTimestamp = Legacy.PoeNinjaCache.LastRefreshTimestamp;
                            }
                            OverlayController.SetQueryResult(queryResult);
                            return;
                        }

                        OverlayController.Hide();
                    }

                })
            });
            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "DEBUG - League Overlay",
                Command = new RelayCommand(_ => LeagueOverlayController.Show())
            });
            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = "DEBUG - NEW League Overlay",
                Command = new RelayCommand(_ => viewLocator.Open<LeagueView>())
            });
            TrayIcon.ContextMenu.Items.Add(new Separator());
#endif

            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = TrayResources.Settings,
                Command = new RelayCommand(_ => viewLocator.Open<SettingsView>())
            });
            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = TrayResources.ShowLogs,
                Command = new RelayCommand(_ => applicationLogsController.Show())
            });
            TrayIcon.ContextMenu.Items.Add(new Separator());
            TrayIcon.ContextMenu.Items.Add(new MenuItem()
            {
                Header = TrayResources.Exit,
                Command = new RelayCommand(_ => application.Shutdown())
            });
        }

        public void Dispose()
        {
            if (TrayIcon != null)
            {
                TrayIcon.Dispose();
            }

            uiLanguageProvider.UILanguageChanged -= InitContextMenu;
        }
    }
}
