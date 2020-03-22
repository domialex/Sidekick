using System;
using System.Windows.Input;
using Sidekick.Core.Natives;
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
        private readonly INativeClipboard nativeClipboard;

        public TrayIconViewModel(
            App application,
            IViewLocator viewLocator,
            INativeClipboard nativeClipboard)
        {
            this.application = application;
            this.viewLocator = viewLocator;
            this.nativeClipboard = nativeClipboard;
        }

        public ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => viewLocator.Open<ApplicationLogsView>());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());

        public ICommand DebugLeagueOverlayCommand => new RelayCommand(_ => viewLocator.Open<LeagueView>());

        public ICommand DebugCrashCommand => new RelayCommand(_ => throw new Exception("Crash requested via tray icon"));

        #region Debug Price Check
        public ICommand DebugPriceCheckCommand1 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Unique
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

            viewLocator.Open<PriceView>();
        });

        public ICommand DebugPriceCheckCommand2 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Rare
Vengeance Crest
Eternal Burgonet
--------
Quality: +20% (augmented)
Armour: 470 (augmented)
--------
Requirements:
Level: 69
Str: 138
--------
Sockets: R-R-G R 
--------
Item Level: 72
--------
Explosive Arrow deals 25% increased Damage (enchant)
--------
+19 to Armour
+77 to maximum Life
+16% to Fire Resistance
+19% to Chaos Resistance
+26% to Cold Resistance (crafted)
--------
Note: ~price 1 chaos
");

            viewLocator.Open<PriceView>();
        });

        public ICommand DebugPriceCheckCommand3 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Currency
Divine Orb
--------
Stack Size: 2/10
--------
Randomises the numeric values of the random modifiers on an item
--------
Right click this item then left click a magic, rare or unique item to apply it.
Shift click to unstack.
");

            viewLocator.Open<PriceView>();
        });

        public ICommand DebugPriceCheckCommand4 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Normal
The Four Feral Exiles
--------
In a faraway dream, four souls far from home prepare to fight to the death.
--------
You will enter a map that holds four additional Rogue Exiles.
--------
Right-click to add this prophecy to your character.
");

            viewLocator.Open<PriceView>();
        });
        #endregion
    }
}
