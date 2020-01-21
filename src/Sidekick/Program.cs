using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Core.Initialization;
using Sidekick.Core.Update;
using Sidekick.Helpers;
using Sidekick.Helpers.Input;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Windows.Overlay;
using Sidekick.Windows.Prediction;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidekick
{   
    class Program
    {
        public static ServiceProvider ServiceProvider;
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

            var updateManagerService = ServiceProvider.GetService<IUpdateManager>();
            if (Task.Run(() => updateManagerService.NewVersionAvailable()).Result)
            {
                if (MessageBox.Show("There is a new update of Sidekick available. Download and install?", "AutoUpdater", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Task.Run(() => updateManagerService.UpdateSidekick()).Result)
                    {
                        ProcessHelper.mutex = null;
                        MessageBox.Show("Update finished! Restarting Sidekick!", "AutoUpdater", MessageBoxButtons.OK);
                        updateManagerService.Restart();
                    }
                    else
                    {
                        MessageBox.Show("Update failed!");
                    }
                    return;
                }
            }
            Legacy.Initialize();
            TrayIcon.Initialize();

            var initializeService = ServiceProvider.GetService<IInitializer>();
            initializeService.Initialize();

            // Keyboard hooks.
            EventsHandler.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // Price Predictio
            PredictionController.Initialize();

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
                ServiceProvider.Dispose();
                TrayIcon.Dispose();
                EventsHandler.Dispose();
                OverlayController.Dispose();
                MAIN_DISPATCHER = null;
            }
        }
    }
}
