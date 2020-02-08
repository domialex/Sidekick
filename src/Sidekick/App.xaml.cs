using System;
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));       // Tooltip opened indefinitly until mouse is moved

            serviceProvider = Sidekick.Startup.InitializeServices(this);

            Legacy.Initialize(serviceProvider);

            Legacy.ViewLocator.Open<Windows.SplashScreen>();

            await RunAutoUpdate();

            EnsureSingleInstance();

            await serviceProvider.GetService<IInitializer>().Initialize();

            // Overlay.
            OverlayController.Initialize();

            // League Overlay
            LeagueOverlayController.Initialize();

            EventsHandler.Initialize();

            // Price Prediction
            PredictionController.Initialize();
        }

        private async Task RunAutoUpdate()
        {
            var updateManagerService = serviceProvider.GetService<IUpdateManager>();
            if (await updateManagerService.NewVersionAvailable())
            {
                if (MessageBox.Show("There is a new version of Sidekick available. Download and install?", "Sidekick Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (await updateManagerService.UpdateSidekick())
                        {
                            Legacy.NativeProcess.Mutex = null;
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
                    catch (Exception)
                    {
                        MessageBox.Show("Update failed! Please update manually.");
                        Legacy.NativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                    }
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            trayIcon.Dispose();
            serviceProvider.Dispose();
            OverlayController.Dispose();
            PredictionController.Dispose();
            base.OnExit(e);
        }

        private void EnsureSingleInstance()
        {
            Legacy.NativeProcess.Mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out var instanceResult);
            if (!instanceResult)
            {
                Current.Shutdown();
            }
        }
    }
}
