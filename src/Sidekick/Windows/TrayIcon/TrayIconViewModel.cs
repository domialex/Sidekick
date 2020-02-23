using System.Threading.Tasks;
using System.Windows.Input;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.UI.Views;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Leagues;
using Sidekick.Windows.Prices;
using Sidekick.Windows.Settings;

namespace Sidekick.Windows.TrayIcon
{
    public class TrayIconViewModel
    {
        private readonly App application;
        private readonly IViewLocator viewLocator;
        private readonly IItemParser itemParser;
        private readonly ITradeClient tradeClient;
        private readonly OverlayController overlayController;
        private readonly IPoeNinjaCache poeNinjaCache;

        public TrayIconViewModel(
            App application,
            IViewLocator viewLocator,
            IItemParser itemParser,
            ITradeClient tradeClient,
            OverlayController overlayController,
            IPoeNinjaCache poeNinjaCache)
        {
            this.application = application;
            this.viewLocator = viewLocator;
            this.itemParser = itemParser;
            this.tradeClient = tradeClient;
            this.overlayController = overlayController;
            this.poeNinjaCache = poeNinjaCache;
        }

        public ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => viewLocator.Open<ApplicationLogsView>());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());

        public ICommand DebugPriceCheckCommand => new RelayCommand(async _ => await DebugPriceCheck());

        public ICommand DebugLeagueOverlayCommand => new RelayCommand(_ => viewLocator.Open<LeagueView>());

        private async Task DebugPriceCheck()
        {
            var item = await itemParser.ParseItem(@"Rarity: Unique
Blood of the Karui
Sanctified Life Flask
--------
Quality: +20% (augmented)
Recovers 3504 (augmented) Life over 2.60 (augmented) Seconds
Consumes 15 of 30 Charges on use
Currently has 30 Charges
--------
Requirements:
Level: 50
--------
Item Level: 75
--------
100% increased Life Recovered
15% increased Recovery rate
Recover Full Life at the end of the Flask Effect
--------
""Kaom fought and killed for his people.
Kaom bled for his people.
And so the people gave, the people bled,
So their King might go on.""
- Lavianga, Advisor to Kaom
--------
Right click to drink.Can only hold charges while in belt.Refills as you kill monsters.
");

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
                return;
            }
            overlayController.Hide();
        }

    }
}
