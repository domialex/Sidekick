using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;

namespace Sidekick.Helpers
{
    public static class EventsHandler
    {
        private static IKeyboardMouseEvents _globalHook;
        private static Dictionary<Hotkey, Action> _hotkeys;

        public static void Initialize()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHookKeyPressHandler;
            _globalHook.MouseWheelExt += GlobalHookMouseScrollHandler;

            _hotkeys = new Dictionary<Hotkey, Action>();

            // #TODO: Remap all actions to json read local file for allowing user bindings
            var exit = Sequence.FromString("Shift+Z, Shift+Z");
            var assignment = new Dictionary<Sequence, Action>
            {
                { exit, ExitApplication }
            };

            _globalHook.OnSequence(assignment);

            RegisterHotkey(new Hotkey(Keys.None, Keys.Escape, true, HotKeyRestrictions.OverlayDisplayed), new Action(() =>
            {
                OverlayController.Hide();
            }));

            RegisterHotkey(new Hotkey(Keys.Control, Keys.D, true, HotKeyRestrictions.PathOfExileFocused | HotKeyRestrictions.OverlayClosed), TriggerItemFetch);
            RegisterHotkey(new Hotkey(Keys.Alt, Keys.W, true, HotKeyRestrictions.PathOfExileFocused), TriggerItemWiki);
            RegisterHotkey(new Hotkey(Keys.None, Keys.F5, true, HotKeyRestrictions.PathOfExileFocused), TriggerHideout);

        }

        public static void RegisterHotkey(Hotkey hotkey, Action action)
        {
            if (_hotkeys.ContainsKey(hotkey))
                throw new HotkeyInUseException(hotkey);

            _hotkeys.Add(hotkey, action);
        }

        public static void DeregisterHotkey(Hotkey hotkey)
        {
            if (_hotkeys.ContainsKey(hotkey))
                _hotkeys.Remove(hotkey);
        }

        private static void GlobalHookKeyPressHandler(object sender, KeyEventArgs e)
        {
            if (!TradeClient.IsReady)
            {
                return;
            }

            foreach (var hotkey in _hotkeys)
            {
                if (hotkey.Key.MeetsRestrictions() && hotkey.Key.IsPressed(e))
                {
                    e.Handled = hotkey.Key.HandleEvent;
                    Task.Run(hotkey.Value);
                    break;
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
                string key = e.Delta > 0 ? Input.KeyCommands.STASH_LEFT : Input.KeyCommands.STASH_RIGHT;
                SendKeys.Send(key);
            }
        }

        private static async void TriggerItemFetch()
        {
            Logger.Log("Hotkey for pricing item triggered.");

            Item item = await TriggerCopyAction();
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

        private static async void TriggerItemWiki()
        {
            Logger.Log("Hotkey for opening wiki triggered.");

            var item = await TriggerCopyAction();
            if (item != null)
            {
                POEWikiAPI.POEWikiHelper.Open(item);
            }
        }

        /// <summary>
        /// Triggers the goto hideout command and restores the chat to your previous entry
        /// </summary>
        private static void TriggerHideout()
        {
            Logger.Log("Hotkey for Hideout triggered.");

            SendKeys.SendWait(Input.KeyCommands.HIDEOUT);
        }

        private static void ExitApplication()
        {
            Application.Exit();
        }

        private static async Task<Item> TriggerCopyAction()
        {
            // Trigger copy action.
            SendKeys.SendWait(Input.KeyCommands.COPY);
            Thread.Sleep(100);

            // Retrieve clipboard.
            var itemText = ClipboardHelper.GetText();

            // Detect the language of the item in the clipboard.
            var setLanguageSuccess = await LanguageSettings.FindAndSetLanguageProvider(itemText);
            if (!setLanguageSuccess)
            {
                return null;
            }

            // Parse and return item
            return ItemParser.ParseItem(itemText);
        }

        public static void Dispose()
        {
            _globalHook?.Dispose();
        }
    }
}
