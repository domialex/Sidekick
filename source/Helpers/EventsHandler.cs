using Gma.System.MouseKeyHook;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;
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
            //_globalHook.MouseWheelExt += GlobalHookMouseScrollHandler;
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
            else if (ProcessHelper.IsPathOfExileInFocus())
            {
                if (!OverlayController.IsDisplayed && e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
                {
                    e.Handled = true;
                    Task.Run(TriggerItemFetch);
                }
                else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.W)
                {
                    e.Handled = true;
                    Task.Run(TriggerItemWiki);
                }
            } 
        }

        private static void GlobalHookMouseScrollHandler(object sender, MouseEventExtArgs e)
        {
            if (!TradeClient.IsReady || !ProcessHelper.IsPathOfExileInFocus())
            {
                return;
            }

            // Ctrl + Scroll wheel to move between stash tabs.
            if ((System.Windows.Input.Keyboard.Modifiers & System.Windows.Input.ModifierKeys.Control) > 0)
            {
                e.Handled = true;
                string key = e.Delta > 0 ? "{LEFT}" : "{RIGHT}";
                SendKeys.Send(key);
            }
        }

        private static async void TriggerItemFetch()
        {
            Logger.Log("TriggerItemFetch()");

            Item item = TriggerCopyAction();
            if (item != null)
            {
                OverlayController.Open();

                var queryResult = await TradeClient.GetListings(item);

                if (queryResult != null)
                {
                    OverlayController.SetQueryResult(queryResult);
                    return;
                }
            }

            OverlayController.Hide();
        }

        private static void TriggerItemWiki()
        {
            Logger.Log("TriggerItemWiki()");

            var item = TriggerCopyAction();
            if (item != null)
            {
                POEWikiAPI.POEWikiHelper.Open(item);
            }
        }

        private static Item TriggerCopyAction()
        {
            // Trigger copy action.
            SendKeys.SendWait("^{c}");
            Thread.Sleep(100);

            // Retrieve clipboard.
            var itemText = ClipboardHelper.GetText();

            // Detect the language of the item in the clipboard.
            LanguageSettings.DetectLanguage(itemText);       

            // Parse and return item
            return ItemParser.ParseItem(itemText);
        }

        public static void Dispose()
        {
            _globalHook?.Dispose();
        }
    }
}
