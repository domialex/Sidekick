using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hardcodet.Wpf.TaskbarNotification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Presentation.Localization.Application;
using Sidekick.Presentation.Wpf.Views.TrayIcon;

// Enables debug specific markup in XAML
// See: https://stackoverflow.com/a/19940157
#if DEBUG
[assembly: XmlnsDefinition("debug-mode", "Namespace")]
#endif

namespace Sidekick.Presentation.Wpf
{
    /// <summary>
    /// Entry point for the app
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ServiceProvider serviceProvider;
        private ILogger logger;
        private IMediator mediator;
        public TaskbarIcon TrayIcon { get; set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            MainWindow = new SplashScreen.SplashScreen();
            MainWindow.Show();

            base.OnStartup(e);

            AttachErrorHandlers();

            // Tooltip opened indefinitely until mouse is moved.
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

            serviceProvider = Wpf.Startup.InitializeServices(this);

            logger = serviceProvider.GetRequiredService<ILogger<App>>();
            mediator = serviceProvider.GetRequiredService<IMediator>();

            TrayIcon = (TaskbarIcon)FindResource("TrayIcon");
            TrayIcon.DataContext = serviceProvider.GetRequiredService<TrayIconViewModel>();

            MainWindow.Close();
            await mediator.Send(new InitializeCommand(true));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            TrayIcon?.Dispose();
            serviceProvider?.Dispose();
            base.OnExit(e);
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
