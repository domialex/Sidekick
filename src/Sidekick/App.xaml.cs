using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Update;
using Sidekick.Helpers.Input;
using Sidekick.UI.Views;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Prediction;
using Sidekick.Windows.PriceCheck;

namespace Sidekick
{
    /// <summary>
    /// Entry point for the app
    /// </summary>
    public partial class App : Application
    {
        private const string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        private ServiceProvider serviceProvider;
        private OverlayController overlayController;
        private PredictionController predictionController;
        private LeagueOverlayController leagueOverlayController;
        private EventsHandler eventsHandler;
        private INativeProcess nativeProcess;
        private INativeBrowser nativeBrowser;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));       // Tooltip opened indefinitly until mouse is moved

            serviceProvider = Sidekick.Startup.InitializeServices(this);
            nativeProcess = serviceProvider.GetRequiredService<INativeProcess>();
            nativeBrowser = serviceProvider.GetRequiredService<INativeBrowser>();

            Legacy.Initialize(serviceProvider);
            serviceProvider.GetService<IViewLocator>().Open<Windows.SplashScreen>();

            await RunAutoUpdate();

            EnsureSingleInstance();

            var initializer = serviceProvider.GetService<IInitializer>();
            initializer.OnProgress += (a) =>
            {
                if (!Legacy.ViewLocator.IsOpened<Windows.SplashScreen>())
                {
                    Legacy.ViewLocator.Open<Windows.SplashScreen>();
                }
            };
            await initializer.Initialize();

            eventsHandler = serviceProvider.GetRequiredService<EventsHandler>();

            // Overlay.
            overlayController = serviceProvider.GetRequiredService<OverlayController>();

            // League Overlay
            leagueOverlayController = serviceProvider.GetRequiredService<LeagueOverlayController>();

            // Price Prediction
            predictionController = serviceProvider.GetRequiredService<PredictionController>();
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
                            nativeProcess.Mutex = null;
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
                        nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                    }
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            serviceProvider.Dispose();
            eventsHandler.Dispose();
            overlayController.Dispose();
            leagueOverlayController.Dispose();
            predictionController.Dispose();
            base.OnExit(e);
        }

        private void EnsureSingleInstance()
        {
            nativeProcess.Mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out var instanceResult);
            if (!instanceResult)
            {
                Current.Shutdown();
            }
        }
    }
}
