using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;
using Sidekick.Windows.Settings;
using Sidekick.Windows.Settings.Models;

namespace Sidekick.Helpers
{
    public static class EventsHandler
    {
        #region Native Mouse Binding

        [DllImport("User32.dll")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */

        #endregion

        private static IKeyboardMouseEvents _globalHook = null;

        private static volatile bool _handleEvents = true;

        public static void Initialize()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHookKeyPressHandler;
#if Release
            _globalHook.MouseWheelExt += GlobalHookMouseScrollHandler;
#endif
            // #TODO: Remap all actions to json read local file for allowing user bindings
            var exit = Sequence.FromString("Shift+Z, Shift+Z");
            var assignment = new Dictionary<Sequence, Action>
            {
                { exit, ExitApplication }
            };

            _globalHook.OnSequence(assignment);
        }

        private static void GlobalHookKeyPressHandler(object sender, KeyEventArgs e)
        {
            if(SettingsController.IsDisplayed)
            {
                SettingsController.CaptureKeyEvents(e.KeyCode, e.Modifiers);
                return;
            }

            if (!TradeClient.IsReady)
            {
                return;
            }

            // Some commands produce SendKeys which triggers them again. Also a bit scary with rebindable keys
            // This lock should probably be rethought with a more solid implementation
            if (!_handleEvents)
            {
                return;
            }

            var settings = SettingsController.GetSettingsInstance();
            var setting = settings.GetKeybindSetting(e.KeyCode, e.Modifiers);

            //if (OverlayController.IsDisplayed && e.KeyCode == Keys.Escape)
            if (OverlayController.IsDisplayed && setting == KeybindSetting.CloseWindow)
            {
                e.Handled = true;
                OverlayController.Hide();
            }
            else if (ProcessHelper.IsPathOfExileInFocus())
            {
                //if (!OverlayController.IsDisplayed && e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
                if (!OverlayController.IsDisplayed && setting == KeybindSetting.PriceCheck)
                {
                    e.Handled = true;
                    Task.Run(TriggerItemFetch);
                }
                //else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.W)
                else if (setting == KeybindSetting.ItemWiki)
                {
                    e.Handled = true;
                    Task.Run(TriggerItemWiki);
                }
                //else if (e.Modifiers == Keys.None && e.KeyCode == Keys.F5)
                else if (setting == KeybindSetting.Hideout)
                {
                    e.Handled = true;
                    Task.Run(TriggerHideout);
                }
                else if (setting == KeybindSetting.FindItems)
                {
                    e.Handled = true;
                    Task.Run(TriggerFindItem);
                }
                else if (setting == KeybindSetting.LeaveParty)
                {
                    e.Handled = true;
                    Task.Run(TriggerLeaveParty);
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

        /// <summary>
        /// Kick yourself from the current party
        /// </summary>
        private static void TriggerLeaveParty()
        {
            Logger.Log("Hotkey for leaving party triggered");

            // this operation is only valid if the user has added their character name to the settings file
            string name = string.Empty;
            SettingsController.GetSettingsInstance().GeneralSettings.TryGetValue(GeneralSetting.CharacterName, out name);
            if (string.IsNullOrEmpty(name))
            {
                Logger.Log("This command requires a \"CharacterName\" to be specified in the settings menu.", LogState.Warning);
                return;
            }

            SendKeys.SendWait(Input.KeyCommands.LEAVE_PARTY.Replace("{name}", name));
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private static async void TriggerFindItem()
        {
            // Unregister input listeners to not end up in infinite loop since we are sending ctrl+f in the command here
            _handleEvents = false;

            Logger.Log("Hotkey for finding item triggered.");

            Item item = null;
            string clipboardContents = ClipboardHelper.GetText();
            bool restoreClipboard = true;
            if (!string.IsNullOrEmpty(clipboardContents))
            {
                // check if clipboard contains an old item copy
                // if so, we should not restore the clipboard to previous item
                item = ItemParser.ParseItem(clipboardContents);
                restoreClipboard = item == null && clipboardContents != null;
                item = null;
            }

            // we still need to fetch the item under the cursor if any and make sure we don't use old contents
            ClipboardHelper.SetText(string.Empty);
            item = await TriggerCopyAction();
            if (item != null)
            {
                // #TODO: trademacro has a lot of fine graining and modifiers when searching specific items like map tier or type of item
                string searchText = item.Name;
                ClipboardHelper.SetText(searchText);

                SendKeys.SendWait(Input.KeyCommands.FIND_ITEMS);
            }

            Thread.Sleep(250);

            if (restoreClipboard)
                ClipboardHelper.SetText(clipboardContents);

            _handleEvents = true;
        }

        public static async void TriggerItemWiki()
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
            if (_globalHook != null)
            {
                _globalHook.KeyDown -= GlobalHookKeyPressHandler;
                _globalHook.MouseWheelExt -= GlobalHookMouseScrollHandler;
                _globalHook.Dispose();
            }
        }
    }
}
