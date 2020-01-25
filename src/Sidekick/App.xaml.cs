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
using System.ComponentModel;

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

        private Windows.SplashScreen _splashScreen = new Windows.SplashScreen();

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EnsureSingleInstance();

            _splashScreen.UpdateProgress("Initializing Providers...", 0);
            _splashScreen.Show();
           
            serviceProvider = Sidekick.Startup.InitializeServices();

            _splashScreen.UpdateProgress("Initializing Tray Icon...", 10);
            Legacy.Initialize(serviceProvider);

            trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            trayIcon.DataContext = serviceProvider.GetService<ITrayIconViewModel>();

            _splashScreen.UpdateProgress("Initializing Services...", 20);
            var initializeService = serviceProvider.GetService<IInitializer>();
            await initializeService.Initialize();

            _splashScreen.UpdateProgress("Initializing Handlers...", 30);
            // Keyboard hooks.
            EventsHandler.Initialize();

            _splashScreen.UpdateProgress("Initializing Controllers...", 40);
            // Overlay.
            OverlayController.Initialize();

            // Price Prediction
            PredictionController.Initialize();
            await RunAutoUpdate();
            _splashScreen.UpdateProgress("Finished!", 100);
            _splashScreen.Close();
        }

        private async Task RunAutoUpdate()
        {
            _splashScreen.UpdateProgress("Checking for Updates...", 50);
            var updateManagerService = serviceProvider.GetService<IUpdateManager>();
            if (await updateManagerService.NewVersionAvailable())
            {
                if (MessageBox.Show("There is a new update of Sidekick available. Download and install?", "Sidekick Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _splashScreen.UpdateProgress("Updating Sidekick...", 70);
                    if (await updateManagerService.UpdateSidekick())
                    {                      
                        mutex = null;
                        ProcessHelper.mutex = null;
                        _splashScreen.UpdateProgress("Update finished!", 100);
                        MessageBox.Show("Update finished! Restarting Sidekick!", "Sidekick Update", MessageBoxButton.OK);
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
            _splashScreen = null;
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
