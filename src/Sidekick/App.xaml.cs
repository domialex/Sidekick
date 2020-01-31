using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Update;
using Sidekick.Helpers.Input;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Overlay;
using Sidekick.Windows.Prediction;
using Sidekick.Windows.TrayIcon;

namespace Sidekick
{
    /// <summary>
    /// Entry point for the app
    /// </summary>
    public partial class App : Application
    {
        private const string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        private ServiceProvider serviceProvider;
        private static TaskbarIcon trayIcon;

        private Windows.SplashScreen _splashScreen = new Windows.SplashScreen();

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));       // Tooltip opened indefinitly until mouse is moved
            _splashScreen.UpdateProgress("Initializing Providers...", 0);
            _splashScreen.Show();

            serviceProvider = Sidekick.Startup.InitializeServices();

            Legacy.Initialize(serviceProvider);

            EnsureSingleInstance();

            trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            trayIcon.DataContext = serviceProvider.GetService<ITrayIconViewModel>();

            _splashScreen.UpdateProgress("Initializing Services...", 50);
            var initializeService = serviceProvider.GetService<IInitializer>();
            await initializeService.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // League Overlay
            LeagueOverlayController.Initialize();

            EventsHandler.Initialize();

            // Price Prediction
            PredictionController.Initialize();

            _splashScreen.UpdateProgress("Initialized!", 100);

            await RunAutoUpdate();
            _splashScreen.Close();
        }

        private async Task RunAutoUpdate()
        {
            var updateManagerService = serviceProvider.GetService<IUpdateManager>();
            updateManagerService.ReportProgress = _splashScreen.UpdateProgress;
            if (await updateManagerService.NewVersionAvailable())
            {
                if (MessageBox.Show("There is a new version of Sidekick available. Download and install?", "Sidekick Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (await updateManagerService.UpdateSidekick())
                    {
                        Legacy.NativeProcess.Mutex = null;
                        _splashScreen.UpdateProgress("Update finished!", 100);
                        MessageBox.Show("Update finished! Restarting Sidekick!", "Sidekick Update", MessageBoxButton.OK);

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(updateManagerService.InstallDirectory, "Sidekick.exe"),
                            UseShellExecute = false,
                        };
                        Process.Start(startInfo);
                    }
                    else
                    {
                        MessageBox.Show("Update failed!");
                    }

                    Current.Shutdown();
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _splashScreen = null;
            trayIcon.Dispose();
            serviceProvider.Dispose();
            OverlayController.Dispose();
            PredictionController.Dispose();
            base.OnExit(e);
        }

        private void EnsureSingleInstance()
        {
            Legacy.NativeProcess.Mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out bool instanceResult);
            if (!instanceResult)
            {
                Current.Shutdown();
            }
        }

        public static void ShowNotifcation(string title, string text = null)
        {
            trayIcon.ShowBalloonTip(title, text, trayIcon.Icon, largeIcon: true);
        }
    }
}
