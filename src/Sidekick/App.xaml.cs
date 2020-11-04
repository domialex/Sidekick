using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hardcodet.Wpf.TaskbarNotification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Natives;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Localization.Application;
using Sidekick.Localization.Splash;
using Sidekick.Presentation.App;
using Sidekick.Views.TrayIcon;

// Enables debug specific markup in XAML
// See: https://stackoverflow.com/a/19940157
#if DEBUG
[assembly: XmlnsDefinition("debug-mode", "Namespace")]
#endif

namespace Sidekick
{
    /// <summary>
    /// Entry point for the app
    /// </summary>
    public partial class App : System.Windows.Application, INativeApp
    {
        private const string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        private ServiceProvider serviceProvider;
        private ILogger logger;
        private INativeProcess nativeProcess;
        private IMediator mediator;
        public TaskbarIcon TrayIcon { get; set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AttachErrorHandlers();

            // Tooltip opened indefinitely until mouse is moved.
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

            serviceProvider = Sidekick.Startup.InitializeServices(this);

            logger = serviceProvider.GetRequiredService<ILogger<App>>();
            nativeProcess = serviceProvider.GetRequiredService<INativeProcess>();
            mediator = serviceProvider.GetRequiredService<IMediator>();

            TrayIcon = (TaskbarIcon)FindResource("TrayIcon");
            TrayIcon.DataContext = serviceProvider.GetRequiredService<TrayIconViewModel>();

            EnsureSingleInstance();
            await mediator.Send(new InitializeCommand(true));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TrayIcon?.Dispose();
            serviceProvider?.Dispose();
            base.OnExit(e);
        }

        private void EnsureSingleInstance()
        {
            nativeProcess.Mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out var instanceResult);
            if (!instanceResult)
            {
                AdonisUI.Controls.MessageBox.Show(SplashResources.AlreadyRunningText, SplashResources.AlreadyRunningTitle, AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        private void AttachErrorHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                LogUnhandledException(exception);
            };

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception);
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception);
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception ex)
        {
            logger.LogCritical(ex, "Unhandled exception in application root");
            Dispatcher.Invoke(() =>
            {
                try
                {
                    // Try to dispose the provider before shutting down
                    try
                    {
                        serviceProvider.Dispose();
                    }
                    catch (Exception)
                    {
                        // Nothing
                    }

                    AdonisUI.Controls.MessageBox.Show(ApplicationResources.FatalErrorOccured, buttons: AdonisUI.Controls.MessageBoxButton.OK);
                }
                catch (Exception)
                {
                    // Sometimes showing the MessageBox causes an Exception.
                    // At this point, there is nothing to do before shutting down the application.
                }
                Shutdown(1);
            });
        }
    }
}
