using System;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Hardcodet.Wpf.TaskbarNotification;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Core.Initialization;
using Sidekick.Windows.TrayIcon;
using Sidekick.Windows.Overlay;
using Sidekick.Helpers.Input;
using Sidekick.Windows.Prediction;
using Sidekick.Core.Update;
using System.Threading.Tasks;

namespace Sidekick
{
    /// <summary>
    /// Entry point for the app
    /// </summary>
    public partial class App : Application
    {
        private const string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        private Mutex mutex;

        private ServiceProvider serviceProvider;
        private static TaskbarIcon trayIcon;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EnsureSingleInstance();

            serviceProvider = Sidekick.Startup.InitializeServices();

            await RunAutoUpdate();

            Legacy.Initialize(serviceProvider);

            trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            trayIcon.DataContext = serviceProvider.GetService<ITrayIconViewModel>();

            var initializeService = serviceProvider.GetService<IInitializer>();
            await initializeService.Initialize();

            // Keyboard hooks.
            EventsHandler.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // Price Prediction
            PredictionController.Initialize();
        }

        private async Task RunAutoUpdate()
        {
            var updateManagerService = serviceProvider.GetService<IUpdateManager>();
            if (await updateManagerService.NewVersionAvailable())
            {
                if (MessageBox.Show("There is a new update of Sidekick available. Download and install?", "AutoUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (await updateManagerService.UpdateSidekick())
                    {
                        mutex = null;
                        ProcessHelper.mutex = null;
                        MessageBox.Show("Update finished! Restarting Sidekick!", "AutoUpdater", MessageBoxButton.OK);
                        updateManagerService.Restart();
                    }
                    else
                    {
                        MessageBox.Show("Update failed!");
                    }

                    App.Current.Shutdown();
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            trayIcon.Dispose();
            serviceProvider.Dispose();
            EventsHandler.Dispose();
            OverlayController.Dispose();
            PredictionController.Dispose();
            base.OnExit(e);
        }

        private void EnsureSingleInstance()
        {
            mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out bool instanceResult);
            if (!instanceResult)
            {
                App.Current.Shutdown();
            }

            ProcessHelper.mutex = mutex;
        }

        public static void ShowNotifcation(string title, string text = null)
        {
            trayIcon.ShowBalloonTip(title, text, trayIcon.Icon, largeIcon: true);
        }
    }
}
