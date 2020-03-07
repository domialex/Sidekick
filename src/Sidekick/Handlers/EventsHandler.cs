using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis;
using Sidekick.Business.Chat;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Business.Whispers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
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
        private readonly IChatService chatService;
        private readonly SidekickSettings settings;
        private bool isDisposed;

        public EventsHandler(
            IKeybindEvents events,
            IWhisperService whisperService,
            INativeClipboard clipboard,
            INativeKeyboard keyboard,
            IItemParser itemParser,
            ILogger logger,
            ITradeClient tradeClient,
            IWikiProvider wikiProvider,
            IViewLocator viewLocator,
            IChatService chatService,
            SidekickSettings settings)
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
            this.chatService = chatService;
            this.settings = settings;
            Initialize();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                events.OnItemWiki -= TriggerItemWiki;
                events.OnFindItems -= TriggerFindItem;
                events.OnLeaveParty -= TriggerLeaveParty;
                events.OnOpenSearch -= TriggerOpenSearch;
                events.OnWhisperReply -= TriggerReplyToLatestWhisper;
                events.OnOpenLeagueOverview -= Events_OnOpenLeagueOverview;
                events.OnPriceCheck -= Events_OnPriceCheck;
                events.OnHideout -= Events_OnHideout;
                events.OnExit -= Events_OnExit;
            }

            isDisposed = true;
        }

        private async Task<bool> Events_OnHideout()
        {
            await chatService.Write("/hideout");
            return true;
        }

        private void Initialize()
        {
            events.OnItemWiki += TriggerItemWiki;
            events.OnFindItems += TriggerFindItem;
            events.OnLeaveParty += TriggerLeaveParty;
            events.OnOpenSearch += TriggerOpenSearch;
            events.OnWhisperReply += TriggerReplyToLatestWhisper;
            events.OnOpenLeagueOverview += Events_OnOpenLeagueOverview;
            events.OnPriceCheck += Events_OnPriceCheck;
            events.OnHideout += Events_OnHideout;
            events.OnExit += Events_OnExit;
        }

        private async Task<bool> Events_OnExit()
        {
            await chatService.Write("/exit");
            return true;
        }

        private async Task<bool> Events_OnPriceCheck()
        {
            viewLocator.CloseAll();
            await clipboard.Copy();
            viewLocator.Open<PriceView>();
            return true;
        }

        private Task<bool> Events_OnOpenLeagueOverview()
        {
            viewLocator.CloseAll();
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
        private async Task<bool> TriggerLeaveParty()
        {
            // This operation is only valid if the user has added their character name to the settings file.
            if (string.IsNullOrEmpty(settings.Character_Name))
            {
                logger.LogWarning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
                return false;
            }
            await chatService.Write($"/kick {settings.Character_Name}");
            return true;
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
                logger.LogInformation(item.Name);
                await clipboard.SetText(searchText);

                keyboard.SendInput("Ctrl+F");
                keyboard.SendInput("Ctrl+A");
                keyboard.Paste();
                keyboard.SendInput("Enter");
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
                return await itemParser.ParseItem(text);
            }

            return null;
        }
    }
}
