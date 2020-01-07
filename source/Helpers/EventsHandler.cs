using Gma.System.MouseKeyHook;
using Sidekick.Helpers;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidekick.Helpers
{
    public static class EventsHandler
    {
        private static IKeyboardMouseEvents _globalHook;

        public static void Initialize()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHookKeyPressHandler;
        }

        private static void GlobalHookKeyPressHandler(object sender, KeyEventArgs e)
        {
            if (!TradeClient.IsReady)
            {
                return;
            }

            if (OverlayController.IsDisplayed && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                OverlayController.Hide();
            }
            else if (!OverlayController.IsDisplayed && e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
            {
                if (!ProcessHelper.IsPathOfExileInFocus())
                    return;

                e.Handled = true;
                Task.Run(TriggerItemFetch);
            }
        }

        private static async void TriggerItemFetch()
        {
            Logger.Log("Hotkey pressed.");

            // Trigger copy action.
            SendKeys.SendWait("^{c}");
            Thread.Sleep(100);
            // Retrieve clipboard.
            var itemText = ClipboardHelper.GetText();
            // Parse item.            
            var item = ItemParser.ParseItem(itemText);
            if (item != null)
            {
                OverlayController.SetPosition(Cursor.Position.X, Cursor.Position.Y);
                OverlayController.Show();

                var queryResult = await TradeClient.GetListings(item);
                if (queryResult != null)
                {
                    OverlayController.SetQueryResult(queryResult);
                    return;
                }
            }

            OverlayController.Hide();
        }

        public static void Dispose()
        {
            _globalHook?.Dispose();
        }
    }
}
