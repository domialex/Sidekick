using System;
using System.Threading.Tasks;
using Sidekick.Business.Apis;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Business.Whispers;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.UI.Views;
using Sidekick.Windows.Leagues;
using Sidekick.Windows.Prices;

namespace Sidekick.Handlers
{
    public class EventsHandler : IDisposable
    {
        private readonly IKeybindEvents events;
        private readonly IWhisperService whisperService;
        private readonly INativeClipboard clipboard;
        private readonly INativeKeyboard keyboard;
        private readonly IItemParser itemParser;
        private readonly ILogger logger;
        private readonly ITradeClient tradeClient;
        private readonly IWikiProvider wikiProvider;
        private readonly IViewLocator viewLocator;

        public EventsHandler(
            IKeybindEvents events,
            IWhisperService whisperService,
            INativeClipboard clipboard,
            INativeKeyboard keyboard,
            IItemParser itemParser,
            ILogger logger,
            ITradeClient tradeClient,
            IWikiProvider wikiProvider,
            IViewLocator viewLocator)
        {
            this.events = events;
            this.whisperService = whisperService;
            this.clipboard = clipboard;
            this.keyboard = keyboard;
            this.itemParser = itemParser;
            this.logger = logger;
            this.tradeClient = tradeClient;
            this.wikiProvider = wikiProvider;
            this.viewLocator = viewLocator;
            Initialize();
        }

        public void Dispose()
        {
            events.OnItemWiki -= TriggerItemWiki;
            events.OnHideout -= TriggerHideout;
            events.OnFindItems -= TriggerFindItem;
            events.OnLeaveParty -= TriggerLeaveParty;
            events.OnOpenSearch -= TriggerOpenSearch;
            events.OnWhisperReply -= TriggerReplyToLatestWhisper;
            events.OnOpenLeagueOverview -= TriggerLeagueOverview;
            events.OnPriceCheck -= Events_OnPriceCheck;
        }

        private void Initialize()
        {
            events.OnItemWiki += TriggerItemWiki;
            events.OnHideout += TriggerHideout;
            events.OnFindItems += TriggerFindItem;
            events.OnLeaveParty += TriggerLeaveParty;
            events.OnOpenSearch += TriggerOpenSearch;
            events.OnWhisperReply += TriggerReplyToLatestWhisper;
            events.OnOpenLeagueOverview += TriggerLeagueOverview;
            events.OnPriceCheck += Events_OnPriceCheck;
        }

        private async Task<bool> Events_OnPriceCheck()
        {
            await clipboard.Copy();
            viewLocator.Open<PriceView>();
            return true;
        }

        private Task<bool> TriggerLeagueOverview()
        {
            viewLocator.Open<LeagueView>();
            return Task.FromResult(true);
        }

        private Task<bool> TriggerReplyToLatestWhisper()
        {
            return whisperService.ReplyToLatestWhisper();
        }

        /// <summary>
        /// Kick yourself from the current party
        /// </summary>
        private Task<bool> TriggerLeaveParty()
        {
            keyboard.SendCommand(KeyboardCommandEnum.LeaveParty);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private async Task<bool> TriggerFindItem()
        {
            var item = await TriggerCopyAction();
            if (item != null)
            {
                var clipboardContents = await clipboard.GetText();

                // #TODO: trademacro has a lot of fine graining and modifiers when searching specific items like map tier or type of item
                var searchText = item.Name;
                logger.Log(item.Name);
                await clipboard.SetText(searchText);

                keyboard.SendCommand(KeyboardCommandEnum.FindItems);
                await Task.Delay(250);
                await clipboard.SetText(clipboardContents);

                return true;
            }

            return false;
        }

        private async Task<bool> TriggerItemWiki()
        {
            var item = await TriggerCopyAction();

            if (item != null)
            {
                wikiProvider.Open(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Triggers the goto hideout command and restores the chat to your previous entry
        /// </summary>
        private Task<bool> TriggerHideout()
        {
            keyboard.SendCommand(KeyboardCommandEnum.GoToHideout);
            return Task.FromResult(true);
        }

        private async Task<bool> TriggerOpenSearch()
        {
            var item = await TriggerCopyAction();

            if (item != null)
            {
                await tradeClient.OpenWebpage(item);
                return true;
            }

            return false;
        }

        private async Task<Business.Parsers.Models.Item> TriggerCopyAction()
        {
            var text = await clipboard.Copy();

            if (!string.IsNullOrWhiteSpace(text))
            {
                return await itemParser.ParseItem(text, false);
            }

            return null;
        }
    }
}
