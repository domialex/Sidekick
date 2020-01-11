using Sidekick.Helpers;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sidekick
{
    class Program
    {
        static readonly string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        [STAThread]
        static void Main()
        {
            // Only have one instance running.
            var mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out bool instanceResult);
            if (!instanceResult) return;
            GC.KeepAlive(mutex);

            Logger.Log("Starting Sidekick.");

            var settingsHandler = new SettingsHandler();
            settingsHandler.ReadSettings();
            settingsHandler.HandleCurrentSettings();

            // System tray icon.
            TrayIcon.Initialize();

            // Load POE Trade information.
            TradeClient.Initialize();

            // Keyboard hooks.
            EventsHandler.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // Run window.
            Application.ApplicationExit += (s, args) => OnApplicationExit(s, args, settingsHandler);        // Maybe find a better way
            Application.Run();
        }

        private static void OnApplicationExit(object sender, EventArgs e, SettingsHandler handler)
        {
            TrayIcon.Dispose();
            TradeClient.Dispose();
            EventsHandler.Dispose();
            OverlayController.Dispose();
            handler.WriteSettings();
        }
    }
}