using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Windows.PriceCheck;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Windows.AdvancedSearch
{
    public class AdvancedSearchController
    {
        private readonly INativeClipboard clipboard;
        private readonly ILogger logger;
        private readonly IItemParser itemParser;
        private readonly ITradeClient tradeClient;
        private readonly OverlayController overlayController;

        public AdvancedSearchController(INativeClipboard clipboard, IKeybindEvents events, ILogger logger, IItemParser itemParser, ITradeClient tradeClient, IServiceProvider serviceProvider)
        {
            this.clipboard = clipboard;
            this.logger = logger;
            this.itemParser = itemParser;
            this.tradeClient = tradeClient;
            overlayController = serviceProvider.GetService<OverlayController>();

            events.OnCloseWindow += OnCloseWindow;
            events.OnAdvancedSearch += OnAdvancedSearch;
        }

        private Task<bool> OnCloseWindow()
        {
            return Task.FromResult(true);
        }

        private async Task<bool> OnAdvancedSearch()
        {
            logger.Log("Hotkey for advanced serach triggered");

            var text = await clipboard.Copy();

            if(!string.IsNullOrWhiteSpace(text))
            {
                var item = await itemParser.ParseItem(text, true);

                if(item != null)
                {
                    // Build Window
                    // Get Item

                    return await overlayController.PriceCheckItem(item);
                }
            }

            // Close
            return false;
        }
    }
}
