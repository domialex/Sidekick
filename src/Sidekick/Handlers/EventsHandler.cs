using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Whispers;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Keybinds;
using Sidekick.Presentation.Views;

namespace Sidekick.Handlers
{
    public class EventsHandler
    {
        private readonly IWhisperService whisperService;
        private readonly INativeClipboard clipboard;
        private readonly ILogger logger;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IWikiProvider wikiProvider;
        private readonly IViewLocator viewLocator;
        private readonly IParserService parserService;
        private readonly IKeybindsProvider keybindsProvider;

        public EventsHandler(
            IWhisperService whisperService,
            INativeClipboard clipboard,
            ILogger<EventsHandler> logger,
            ITradeSearchService tradeSearchService,
            IWikiProvider wikiProvider,
            IViewLocator viewLocator,
            IParserService parserService,
            IKeybindsProvider keybindsProvider)
        {
            this.whisperService = whisperService;
            this.clipboard = clipboard;
            this.logger = logger;
            this.tradeSearchService = tradeSearchService;
            this.wikiProvider = wikiProvider;
            this.viewLocator = viewLocator;
            this.parserService = parserService;
            this.keybindsProvider = keybindsProvider;
        }

        private async Task<bool> Events_OnPriceCheck()
        {
            viewLocator.CloseAll();
            await clipboard.Copy();

            viewLocator.Open(View.Price);
            return true;
        }

        private async Task<bool> Events_OnMapInfo()
        {
            viewLocator.CloseAll();
            await clipboard.Copy();

            viewLocator.Open(View.Map);
            return true;
        }

        private Task<bool> Events_OnOpenLeagueOverview()
        {
            if (viewLocator.IsOpened(View.League))
            {
                viewLocator.CloseAll();
            }
            else
            {
                viewLocator.CloseAll();
                viewLocator.Open(View.League);
            }

            return Task.FromResult(true);
        }

        private Task<bool> TriggerReplyToLatestWhisper()
        {
            return whisperService.ReplyToLatestWhisper();
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

                logger.LogInformation("Searching for {itemName}", item.Name);
                await clipboard.SetText(item.Name);

                keybindsProvider.PressKey("Ctrl+F");
                keybindsProvider.PressKey("Ctrl+A");
                keybindsProvider.PressKey("Paste");
                keybindsProvider.PressKey("Enter");
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
                await wikiProvider.Open(item);
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

        private Task<bool> TriggerOpenSettings()
        {
            if (viewLocator.IsOpened(View.Settings))
            {
                viewLocator.CloseAll();
            }
            else
            {
                viewLocator.CloseAll();
                viewLocator.Open(View.Settings);
            }

            return Task.FromResult(true);
        }

        private async Task<Business.Apis.Poe.Models.Item> TriggerCopyAction()
        {
            var text = await clipboard.Copy();

            if (!string.IsNullOrWhiteSpace(text))
            {
                return parserService.ParseItem(text);
            }

            return null;
        }
    }
}
