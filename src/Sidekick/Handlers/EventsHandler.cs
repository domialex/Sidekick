using System;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Chat;
using Sidekick.Business.Stashes;
using Sidekick.Business.Whispers;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Views;
using Sidekick.Views.Leagues;
using Sidekick.Views.Prices;

namespace Sidekick.Handlers
{
    public class EventsHandler : IDisposable, IOnReset
    {
        private readonly IKeybindEvents events;
        private readonly IWhisperService whisperService;
        private readonly INativeClipboard clipboard;
        private readonly INativeKeyboard keyboard;
        private readonly ILogger logger;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IWikiProvider wikiProvider;
        private readonly IViewLocator viewLocator;
        private readonly IChatService chatService;
        private readonly IStashService stashService;
        private readonly SidekickSettings settings;
        private readonly IParserService parserService;
        private bool isDisposed;

        public EventsHandler(
            IKeybindEvents events,
            IWhisperService whisperService,
            INativeClipboard clipboard,
            INativeKeyboard keyboard,
            ILogger logger,
            ITradeSearchService tradeSearchService,
            IWikiProvider wikiProvider,
            IViewLocator viewLocator,
            IChatService chatService,
            IStashService stashService,
            SidekickSettings settings,
            IParserService parserService)
        {
            this.events = events;
            this.whisperService = whisperService;
            this.clipboard = clipboard;
            this.keyboard = keyboard;
            this.logger = logger.ForContext(GetType());
            this.tradeSearchService = tradeSearchService;
            this.logger = logger.ForContext(GetType());
            this.wikiProvider = wikiProvider;
            this.viewLocator = viewLocator;
            this.chatService = chatService;
            this.stashService = stashService;
            this.settings = settings;
            this.parserService = parserService;
            Initialize();
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            OnReset();
            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void OnReset()
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
            events.OnTabLeft -= Events_OnTabLeft;
            events.OnTabRight -= Events_OnTabRight;
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
            events.OnTabLeft += Events_OnTabLeft;
            events.OnTabRight += Events_OnTabRight;
        }

        private Task<bool> Events_OnTabRight()
        {
            stashService.ScrollRight();
            return Task.FromResult(true);
        }

        private Task<bool> Events_OnTabLeft()
        {
            stashService.ScrollLeft();
            return Task.FromResult(true);
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
                logger.Warning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
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
                logger.Information("Searching for {itemName}", item.Name);
                await clipboard.SetText(item.Name);

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
                await tradeSearchService.OpenWebpage(item);
                return true;
            }

            return false;
        }

        private async Task<ParsedItem> TriggerCopyAction()
        {
            var text = await clipboard.Copy();

            if (!string.IsNullOrWhiteSpace(text))
            {
                return await parserService.ParseItem(text);
            }

            return null;
        }
    }
}
