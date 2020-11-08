using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Keybinds;

namespace Sidekick.Handlers
{
    public class EventsHandler
    {
        private readonly IClipboardProvider clipboard;
        private readonly ILogger logger;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IWikiProvider wikiProvider;
        private readonly IParserService parserService;
        private readonly IKeybindsProvider keybindsProvider;

        public EventsHandler(
            IClipboardProvider clipboard,
            ILogger<EventsHandler> logger,
            ITradeSearchService tradeSearchService,
            IWikiProvider wikiProvider,
            IParserService parserService,
            IKeybindsProvider keybindsProvider)
        {
            this.clipboard = clipboard;
            this.logger = logger;
            this.tradeSearchService = tradeSearchService;
            this.wikiProvider = wikiProvider;
            this.parserService = parserService;
            this.keybindsProvider = keybindsProvider;
        }

        /// <summary>
        /// Attempts to fill the search field of the stash tab with the current items name if any
        /// </summary>
        private async Task<bool> TriggerFindItem()
        {
            var text = await clipboard.Copy();
            var item = parserService.ParseItem(text);

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
            var text = await clipboard.Copy();
            var item = parserService.ParseItem(text);

            if (item != null)
            {
                await wikiProvider.Open(item);
                return true;
            }

            return false;
        }

        private async Task<bool> TriggerOpenSearch()
        {
            var text = await clipboard.Copy();
            var item = parserService.ParseItem(text);

            if (item != null)
            {
                await tradeSearchService.OpenWebpage(item);
                return true;
            }

            return false;
        }
    }
}
