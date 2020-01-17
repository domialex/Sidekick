using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Loggers;
using Sidekick.Helpers;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Windows.Overlay;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sidekick
{
    class Program
    {
        public static IServiceProvider ServiceProvider;
        static readonly string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";
        public static System.Windows.Threading.Dispatcher MAIN_DISPATCHER { get; private set; } = null;

        [STAThread]
        static void Main()
        {
            // Only have one instance running.
            var mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out bool instanceResult);
            if (!instanceResult) return;
            GC.KeepAlive(mutex);
            ProcessHelper.mutex = mutex;

            ServiceProvider = Startup.InitializeServices();

            Legacy.Initialize();

            var logger = ServiceProvider.GetService<ILogger>();
            logger.Log("Starting Sidekick.");

            // System tray icon.
            TrayIcon.Initialize();

            // Load POE Trade information.
            Legacy.TradeClient.Initialize();

            // Keyboard hooks.
            EventsHandler.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // Run window.
            MAIN_DISPATCHER = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            Application.ThreadExit += ThreadExit;
            Application.Run();
        }

        private static void ThreadExit(object sender, EventArgs e)
        {
            // check that the main thread is about to exit
            if (SynchronizationContext.Current != null)
            {
                TrayIcon.Dispose();
                Legacy.TradeClient.Dispose();
                EventsHandler.Dispose();
                OverlayController.Dispose();
                MAIN_DISPATCHER = null;
            }
        }
    }
}
