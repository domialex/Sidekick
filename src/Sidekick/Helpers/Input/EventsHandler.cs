using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Windows.Overlay;
using Sidekick.Windows.Settings;
using WindowsHook;
using KeyEventArgs = WindowsHook.KeyEventArgs;

namespace Sidekick.Helpers.Input
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
            ProcessHelper.CheckPermission();

            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHookKeyPressHandler;
            if (!Debugger.IsAttached)
            {
                _globalHook.MouseWheelExt += GlobalHookMouseScrollHandler;
            }

            // #TODO: Remap all actions to json read local file for allowing user bindings
            var exit = Sequence.FromString("Shift+Z, Shift+Z");
            var assignment = new Dictionary<Sequence, Action>
            {
                { exit, ExitApplication }
            };

            _globalHook.OnSequence(assignment);
        }

        private static async void GlobalHookKeyPressHandler(object sender, KeyEventArgs e)
        {
            if (SettingsController.IsDisplayed)
            {
                SettingsController.CaptureKeyEvents(e.KeyCode, e.Modifiers);
                return;
            }

            if (!Legacy.InitializeService.IsReady)
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

            if (OverlayController.IsDisplayed && setting == KeybindSetting.CloseWindow)
            {
                e.Handled = true;
                OverlayController.Hide();
            }


            if (ProcessHelper.IsPathOfExileInFocus())
            {
                if (setting == KeybindSetting.PriceCheck)
                {
                    e.Handled = true;
                    await TriggerItemFetch();
                }
                else if (setting == KeybindSetting.ItemWiki)
                {
                    e.Handled = true;
                    await TriggerItemWiki();
                }
                else if (setting == KeybindSetting.Hideout)
                {
                    e.Handled = true;
                    TriggerHideout();
                }
                else if (setting == KeybindSetting.FindItems)
                {
                    e.Handled = true;
                    await TriggerFindItem();
                }
                else if (setting == KeybindSetting.LeaveParty)
                {
                    e.Handled = true;
                    TriggerLeaveParty();
                }
                else if (setting == KeybindSetting.OpenSearch)
                {
                    e.Handled = true;
                    await TriggerOpenSearch();
                }
            }
        }

        private static void GlobalHookMouseScrollHandler(object sender, MouseEventExtArgs e) => Task.Run(() => GlobalHookMouseScrollTask(e));

        private static void GlobalHookMouseScrollTask(MouseEventExtArgs e)
        {
            if (!Legacy.InitializeService.IsReady || !ProcessHelper.IsPathOfExileInFocus())
            {
                return;
            }

            // Ctrl + Scroll wheel to move between stash tabs.
            if (KeyboardState.IsKeyPressed(VirtualKeyStates.VK_CONTROL))
            {
                e.Handled = true; // Right now this won't have any effect.
                string key = e.Delta > 0 ? KeyCommands.STASH_LEFT : KeyCommands.STASH_RIGHT;

                SendKeys.SendWait(key);
            }
        }

        private static async Task TriggerItemFetch()
        {
            Legacy.Logger.Log("Hotkey for pricing item triggered.");

            Business.Parsers.Models.Item item = await TriggerCopyAction();
            if (item != null)
            {
                OverlayController.Open();

                var queryResult = await Legacy.TradeClient.GetListings(item);
                if (queryResult != null)
                {
                    var poeNinjaItem = Legacy.PoeNinjaCache.GetItem(item);
                    if (poeNinjaItem != null)
                    {
                        queryResult.PoeNinjaItem = poeNinjaItem;
                        queryResult.LastRefreshTimestamp = Legacy.PoeNinjaCache.LastRefreshTimestamp;
                    }
                    OverlayController.SetQueryResult(queryResult);
                    return;
                }

                OverlayController.Hide();

            }

        }

        /// <summary>
        /// Kick yourself from the current party
        /// </summary>
        private static void TriggerLeaveParty()
        {
            Legacy.Logger.Log("Hotkey for leaving party triggered");

            // this operation is only valid if the user has added their character name to the settings file
            SettingsController.GetSettingsInstance().GeneralSettings.TryGetValue(GeneralSetting.CharacterName, out var name);
            if (string.IsNullOrEmpty(name))
            {
                Legacy.Logger.Log("This command requires a \"CharacterName\" to be specified in the settings menu.", LogState.Warning);
                return;
            }

            SendKeys.SendWait(KeyCommands.LEAVE_PARTY.Replace("{name}", name));
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private static async Task TriggerFindItem()
        {
            // Unregister input listeners to not end up in infinite loop since we are sending ctrl+f in the command here
            _handleEvents = false;

            Legacy.Logger.Log("Hotkey for finding item triggered.");

            Business.Parsers.Models.Item item = null;
            var clipboardContents = ClipboardHelper.GetText();
            var restoreClipboard = true;

            if (!string.IsNullOrEmpty(clipboardContents))
            {
                // check if clipboard contains an old item copy
                // if so, we should not restore the clipboard to previous item
                item = Legacy.ItemParser.ParseItem(clipboardContents);
                restoreClipboard = item == null && clipboardContents != null;
            }

            // we still need to fetch the item under the cursor if any and make sure we don't use old contents
            await ClipboardHelper.SetText(string.Empty);
            item = await TriggerCopyAction();
            if (item != null)
            {
                // #TODO: trademacro has a lot of fine graining and modifiers when searching specific items like map tier or type of item
                string searchText = item.Name;
                await ClipboardHelper.SetText(searchText);

                SendKeys.SendWait(KeyCommands.FIND_ITEMS);
            }

            await Task.Delay(250);

            if (restoreClipboard)
            {
                await ClipboardHelper.SetText(clipboardContents);
            }

            _handleEvents = true;
        }

        public static async Task TriggerItemWiki()
        {
            Legacy.Logger.Log("Hotkey for opening wiki triggered.");

            var item = await TriggerCopyAction();

            if (item != null)
            {
                SettingsController.GetSettingsInstance().GetWikiAction().Invoke(item);
            }
        }

        /// <summary>
        /// Triggers the goto hideout command and restores the chat to your previous entry
        /// </summary>
        private static void TriggerHideout()
        {
            Legacy.Logger.Log("Hotkey for Hideout triggered.");

            SendKeys.SendWait(KeyCommands.HIDEOUT);
        }

        public static async Task TriggerOpenSearch()
        {
            Legacy.Logger.Log("Hotkey for opening search query in browser triggered.");

            var item = await TriggerCopyAction();
            if (item != null)
            {
                await Legacy.TradeClient.OpenWebpage(item);
            }
        }

        private static void ExitApplication()
        {
            Application.Exit();
        }

        private static async Task<Business.Parsers.Models.Item> TriggerCopyAction()
        {
            bool retain = bool.Parse(SettingsController.GetSettingsInstance()?.GeneralSettings[GeneralSetting.RetainClipboard] ?? "false");
            var clipboardText = string.Empty;

            if (retain)
            {
                clipboardText = ClipboardHelper.GetText();
            }

            await ClipboardHelper.SetDataObject(string.Empty);

            SendKeys.SendWait(KeyCommands.COPY);

            await Task.Delay(100);

            // Retrieve clipboard.
            var itemText = ClipboardHelper.GetText();

            if (retain)
            {
                await ClipboardHelper.SetDataObject(clipboardText);
            }

            if (string.IsNullOrWhiteSpace(itemText))
            {
                Legacy.Logger.Log("No item detected in the clipboard.");
                return null;
            }

            // Detect the language of the item in the clipboard.
            await Legacy.LanguageProvider.FindAndSetLanguage(itemText);

            // Parse and return item
            return Legacy.ItemParser.ParseItem(itemText);
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
