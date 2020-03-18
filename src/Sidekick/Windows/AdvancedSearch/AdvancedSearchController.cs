using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.UI.Views;

namespace Sidekick.Windows.AdvancedSearch
{
    public class AdvancedSearchController
    {
        private readonly INativeClipboard clipboard;
        private readonly ILogger logger;
        private readonly ITradeSearchService tradeSearchService;
        private readonly INativeProcess nativeProcess;
        private readonly INativeCursor nativeCursor;
        private readonly IViewLocator viewLocator;
        private readonly AdvancedSearchView view;
        private readonly SidekickSettings settings;

        public AdvancedSearchController(INativeClipboard clipboard, IKeybindEvents events, ILogger logger, ITradeSearchService tradeSearchService, IServiceProvider serviceProvider, INativeProcess nativeProcess, SidekickSettings settings, INativeCursor nativeCursor,
            IViewLocator viewLocator)
        {
            this.clipboard = clipboard;
            this.logger = logger;
            this.tradeSearchService = tradeSearchService;
            this.nativeProcess = nativeProcess;
            this.settings = settings;
            this.nativeCursor = nativeCursor;
            this.viewLocator = viewLocator;
            events.OnAdvancedSearch += OnAdvancedSearch;
            view = new AdvancedSearchView(this);
        }

        private async Task<bool> OnAdvancedSearch()
        {
            logger.LogInformation("Hotkey for advanced search triggered");

            var text = await clipboard.Copy();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var item = await itemParser.ParseItem(text, true);

                if (item != null)
                {
                    view.PopulateGrid(item);
                    Open();
                    return true;
                }
            }

            view.HideWindowAndClearData();
            return false;
        }
    }
}
