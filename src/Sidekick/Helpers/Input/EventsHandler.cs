using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Trades;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Overlay;

namespace Sidekick.Helpers.Input
{
    public class EventsHandler
    {
        private readonly OverlayController overlayController;
        private readonly SidekickSettings settings;
        private readonly ITradeClient tradeClient;
        private readonly IPoeNinjaCache poeNinjaCache;

        public EventsHandler(OverlayController overlayController, SidekickSettings settings, ITradeClient tradeClient, IPoeNinjaCache poeNinjaCache)
        {
            this.overlayController = overlayController;
            this.settings = settings;
            this.tradeClient = tradeClient;
            this.poeNinjaCache = poeNinjaCache;
            Initialize();
        }
        public void Initialize()
        {
            Legacy.KeybindEvents.OnCloseWindow += () =>
            {
                var handled = false;

                if (overlayController.IsDisplayed)
                {
                    overlayController.Hide();
                    handled = true;
                }

                if (LeagueOverlayController.IsDisplayed)
                {
                    LeagueOverlayController.Hide();
                    handled = true;
                }

                return Task.FromResult(handled);
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

        private static Task<bool> TriggerReplyToLatestWhisper()
        {
            var characterName = Legacy.WhisperService.GetLatestWhisperCharacterName();
            if (!string.IsNullOrEmpty(characterName))
            {
                Legacy.NativeClipboard.SetText(string.Empty);
                Legacy.NativeClipboard.SetText($"@{characterName} ");
                Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.ReplyToLatestWhisper);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private Task MouseClicked(int x, int y)
        {
            if (!overlayController.IsDisplayed || !settings.CloseOverlayWithMouse) return Task.CompletedTask;

            var overlayPos = overlayController.GetOverlayPosition();
            var overlaySize = overlayController.GetOverlaySize();

            if (x < overlayPos.X || x > overlayPos.X + overlaySize.Width
                || y < overlayPos.Y || y > overlayPos.Y + overlaySize.Height)
            {
                overlayController.Hide();
            }

            return Task.CompletedTask;
        }

        private async Task<bool> TriggerItemFetch()
        {
            Legacy.Logger.Log("Hotkey for pricing item triggered.");

            var item = await TriggerCopyAction();
            if (item != null)
            {
                overlayController.Open();

                var queryResult = await tradeClient.GetListings(item);
                if (queryResult != null)
                {
                    var poeNinjaItem = poeNinjaCache.GetItem(item);
                    if (poeNinjaItem != null)
                    {
                        queryResult.PoeNinjaItem = poeNinjaItem;
                        queryResult.LastRefreshTimestamp = poeNinjaCache.LastRefreshTimestamp;
                    }
                    overlayController.SetQueryResult(queryResult);
                    return true;
                }

                overlayController.Hide();
                return true;
            }

            return false;
        }

        private static Task<bool> TriggerLeagueOverlay()
        {
            LeagueOverlayController.Open();
            LeagueOverlayController.Show();
            return Task.FromResult(true);
        }
        /// <summary>
        /// Kick yourself from the current party
        /// </summary>
        private static Task<bool> TriggerLeaveParty()
        {
            Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.LeaveParty);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private static async Task<bool> TriggerFindItem()
        {
            var restoreClipboard = true;

            var item = await TriggerCopyAction();
            if (item != null)
            {
                var clipboardContents = await Legacy.NativeClipboard.GetText();

                // #TODO: trademacro has a lot of fine graining and modifiers when searching specific items like map tier or type of item
                var searchText = item.Name;
                Legacy.Logger.Log(item.Name);
                await Legacy.NativeClipboard.SetText(searchText);

                Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.FindItems);

                if (restoreClipboard)
                {
                    await Task.Delay(250);
                    await Legacy.NativeClipboard.SetText(clipboardContents);
                }
                return true;
            }

            return false;
        }

        public static async Task<bool> TriggerItemWiki()
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

                return true;
            }

            return false;
        }

        /// <summary>
        /// Triggers the goto hideout command and restores the chat to your previous entry
        /// </summary>
        private static Task<bool> TriggerHideout()
        {
            Legacy.NativeKeyboard.SendCommand(KeyboardCommandEnum.GoToHideout);
            return Task.FromResult(true);
        }

        public static async Task<bool> TriggerOpenSearch()
        {
            var item = await TriggerCopyAction();
            if (item != null)
            {
                await Legacy.TradeClient.OpenWebpage(item);
                return true;
            }

            return false;
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

            // Parse and return item
            return await Legacy.ItemParser.ParseItem(itemText);
        }
    }
}
