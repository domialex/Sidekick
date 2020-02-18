using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Core.Update;
using Sidekick.Helpers.Input;
using Sidekick.Localization.Tray;
using Sidekick.UI.Views;
using Sidekick.Windows.Prediction;
using Sidekick.Windows.PriceCheck;
using Sidekick.Windows.TrayIcon;

// Enables debug specific markup in XAML
// See: https://stackoverflow.com/a/19940157
#if DEBUG
[assembly: XmlnsDefinition( "debug-mode", "Namespace" )]
#endif

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
        private EventsHandler eventsHandler;
        private INativeProcess nativeProcess;
        private INativeBrowser nativeBrowser;
        private IViewLocator viewLocator;

        private TaskbarIcon trayIcon;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));       // Tooltip opened indefinitly until mouse is moved

            serviceProvider = Sidekick.Startup.InitializeServices(this);
            nativeProcess = serviceProvider.GetRequiredService<INativeProcess>();
            nativeBrowser = serviceProvider.GetRequiredService<INativeBrowser>();

            Legacy.Initialize(serviceProvider);
            viewLocator = serviceProvider.GetService<IViewLocator>();
            viewLocator.Open<Windows.SplashScreen>();

            await RunAutoUpdate();

            EnsureSingleInstance();

            var initializer = serviceProvider.GetService<IInitializer>();
            initializer.OnProgress += (a) =>
            {
                if (!viewLocator.IsOpened<Windows.SplashScreen>())
                {
                    viewLocator.Open<Windows.SplashScreen>();
                }
            };
            await initializer.Initialize();

            InitTrayIcon(serviceProvider.GetRequiredService<SidekickSettings>());

            eventsHandler = serviceProvider.GetRequiredService<EventsHandler>();

            // Overlay.
            overlayController = serviceProvider.GetRequiredService<OverlayController>();

            // Price Prediction
            predictionController = serviceProvider.GetRequiredService<PredictionController>();
        }

        private void InitTrayIcon(SidekickSettings settings)
        {
            trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            trayIcon.DataContext = serviceProvider.GetRequiredService<TrayIconViewModel>();

            trayIcon.ShowBalloonTip(
                    TrayResources.Notification_Title,
                    string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString()),
                    trayIcon.Icon,
                    largeIcon: true);
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
            trayIcon.Dispose();
            // Disposing the service provider also disposes registered all IDisposable services
            serviceProvider.Dispose();
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
