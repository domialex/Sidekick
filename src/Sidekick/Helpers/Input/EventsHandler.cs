using System.Threading.Tasks;
using Sidekick.Core.Natives;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Overlay;

namespace Sidekick.Helpers.Input
{
    public static class EventsHandler
    {
        public static void Initialize()
        {
            Legacy.KeybindEvents.OnCloseWindow += () =>
            {
                if (OverlayController.IsDisplayed)
                {
                    OverlayController.Hide();
                }

                if(LeagueOverlayController.IsDisplayed)
                {
                    LeagueOverlayController.Hide();
                }

                return Task.CompletedTask;
            };

            Legacy.KeybindEvents.OnPriceCheck += TriggerItemFetch;
            Legacy.KeybindEvents.OnItemWiki += TriggerItemWiki;
            Legacy.KeybindEvents.OnHideout += TriggerHideout;
            Legacy.KeybindEvents.OnFindItems += TriggerFindItem;
            Legacy.KeybindEvents.OnLeaveParty += TriggerLeaveParty;
            Legacy.KeybindEvents.OnOpenSearch += TriggerOpenSearch;
            Legacy.KeybindEvents.OnOpenLeagueOverview += TriggerLeagueOverlay;
            Legacy.KeybindEvents.OnWhisperReply += TriggerReplyToLatestWhisper;
            Legacy.KeybindEvents.OnMouseClick += MouseClicked;
        }

        private static Task TriggerReplyToLatestWhisper()
        {
            var characterName = Legacy.WhisperService.GetLatestWhisperCharacterName();
            if(!string.IsNullOrEmpty(characterName))
            {
                Legacy.NativeClipboard.SetText(string.Empty);
                Legacy.NativeClipboard.SetText($"@{characterName} ");
                Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.ReplyToLatestWhisper);
            }
            return Task.CompletedTask;
        }

        private static Task MouseClicked(int x, int y)
        {
            if (!OverlayController.IsDisplayed || !Legacy.Settings.CloseOverlayWithMouse) return Task.CompletedTask;

            var overlayPos = OverlayController.GetOverlayPosition();
            var overlaySize = OverlayController.GetOverlaySize();

            if (x < overlayPos.X || x > overlayPos.X + overlaySize.Width
                || y < overlayPos.Y || y > overlayPos.Y + overlaySize.Height)
            {
                OverlayController.Hide();
            }

            return Task.CompletedTask;
        }

        private static async Task TriggerItemFetch()
        {
            Legacy.Logger.Log("Hotkey for pricing item triggered.");

            var item = await TriggerCopyAction();
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

        private static Task TriggerLeagueOverlay()
        {
            LeagueOverlayController.Open();
            LeagueOverlayController.Show();
            return Task.CompletedTask;
        }
        /// <summary>
        /// Kick yourself from the current party
        /// </summary>
        private static Task TriggerLeaveParty()
        {
            Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.LeaveParty);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private static async Task TriggerFindItem()
        {
            Business.Parsers.Models.Item item;
            var clipboardContents = await Legacy.NativeClipboard.GetText();
            var restoreClipboard = true;

            if (!string.IsNullOrEmpty(clipboardContents))
            {
                // check if clipboard contains an old item copy
                // if so, we should not restore the clipboard to previous item
                item = Legacy.ItemParser.ParseItem(clipboardContents);
                restoreClipboard = item == null && clipboardContents != null;
            }

            // we still need to fetch the item under the cursor if any and make sure we don't use old contents
            await Legacy.NativeClipboard.SetText(string.Empty);
            item = await TriggerCopyAction();
            if (item != null)
            {
                // #TODO: trademacro has a lot of fine graining and modifiers when searching specific items like map tier or type of item
                var searchText = item.Name;
                await Legacy.NativeClipboard.SetText(searchText);

                Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.FindItems);
            }

            await Task.Delay(250);

            if (restoreClipboard)
            {
                await Legacy.NativeClipboard.SetText(clipboardContents);
            }
        }

        public static async Task TriggerItemWiki()
        {
            var item = await TriggerCopyAction();

            if (item != null)
            {
                if (Legacy.Settings.Wiki_Preferred == Core.Settings.WikiSetting.PoeDb)
                {
                    Legacy.PoeDbClient.Open(item);
                }
                else
                {
                    Legacy.PoeWikiClient.Open(item);
                }
            }
        }

        /// <summary>
        /// Triggers the goto hideout command and restores the chat to your previous entry
        /// </summary>
        private static Task TriggerHideout()
        {
            Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.GoToHideout);
            return Task.CompletedTask;
        }

        public static async Task TriggerOpenSearch()
        {
            var item = await TriggerCopyAction();
            if (item != null)
            {
                await Legacy.TradeClient.OpenWebpage(item);
            }
        }

        private static async Task<Business.Parsers.Models.Item> TriggerCopyAction()
        {
            var clipboardText = string.Empty;

            if (Legacy.Settings.RetainClipboard)
            {
                clipboardText = await Legacy.NativeClipboard.GetText();
            }

            await Legacy.NativeClipboard.SetText(string.Empty);

            Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.Copy);

            await Task.Delay(100);

            // Retrieve clipboard.
            var itemText = await Legacy.NativeClipboard.GetText();

            if (Legacy.Settings.RetainClipboard)
            {
                await Legacy.NativeClipboard.SetText(clipboardText);
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
    }
}
