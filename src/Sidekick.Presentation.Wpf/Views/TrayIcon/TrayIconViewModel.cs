using System;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Wpf.Views.TrayIcon
{
    public class TrayIconViewModel
    {
        private readonly App application;
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IMediator mediator;

        public TrayIconViewModel(
            App application,
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider,
            IMediator mediator)
        {
            this.application = application;
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
        }

        public System.Windows.Input.ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open(View.Settings));

        public System.Windows.Input.ICommand ShowAboutCommand => new RelayCommand(_ => viewLocator.Open(View.About));

        public System.Windows.Input.ICommand ShowLogsCommand => new RelayCommand(_ => viewLocator.Open(View.Logs));

        public System.Windows.Input.ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());

        public System.Windows.Input.ICommand DebugLeagueOverlayCommand => new RelayCommand(_ => viewLocator.Open(View.League));

        public static System.Windows.Input.ICommand DebugCrashCommand => new RelayCommand(_ => throw new Exception("Crash requested via tray icon"));

        #region Debug Price Check
        public System.Windows.Input.ICommand DebugPriceCheckCommand0 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(await clipboardProvider.GetText()));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand1 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Unique
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
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand2 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Rare
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
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand3 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Currency
Divine Orb
--------
Stack Size: 2/10
--------
Randomises the numeric values of the random modifiers on an item
--------
Right click this item then left click a magic, rare or unique item to apply it.
Shift click to unstack.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand4 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Normal
The Four Feral Exiles
--------
In a faraway dream, four souls far from home prepare to fight to the death.
--------
You will enter a map that holds four additional Rogue Exiles.
--------
Right-click to add this prophecy to your character.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand5 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rareté: Unique
Assaut de Farrul
Gantelets en écailles d'hydre
--------
Qualité: +20% (augmented)
Armure: 254 (augmented)
Score d'Évasion: 254 (augmented)
--------
Prérequis::
Niveau: 70
For: 111
Dex: 151
--------
Châsses: G-R-R-G 
--------
Niveau de l'objet: 80
--------
120% d'Augmentation de l'Armure et de l'Évasion
+63 de Vie maximale
+488 à la Précision contre les Ennemis qui Saignent
Vos Attaques infligent toujours le Saignement tant que vous avez Discrétion du Félin
41% d'Augmentation des Dégâts au Toucher et avec les Altérations contre les Ennemis qui Saignent
Vous avez Danse écarlate tant que vous avez Discrétion du Félin
--------
Une bête affamée ne gaspille jamais son énergie.
Chaque coup, quelle que soit sa force, doit être porté en vue de la victoire.
La Première des Plaines nous enseigne que même
la plus grande des proies peut être éventuellement déchiquetée.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand6 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Rare
Death Nails
Assassin's Mitts
--------
Evasion Rating: 104
Energy Shield: 20
--------
Requirements:
Level: 58
Dex: 45
Int: 45
--------
Sockets: G 
--------
Item Level: 61
--------
+18 to Intelligence
+73 to maximum Life
+14% to Lightning Resistance
0.23% of Physical Attack Damage Leeched as Mana
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand7 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Rare
Blight Cut
Cobalt Jewel
--------
Item Level: 68
--------
+8 to Strength and Intelligence
14% increased Spell Damage while Dual Wielding
19% increased Burning Damage
15% increased Damage with Wands
--------
Place into an allocated Jewel Socket on the Passive Skill Tree.Right click to remove from the Socket.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand8 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Gem
Double Strike
--------
Vaal, Attack, Melee, Strike, Duration, Physical
Level: 1
Mana Cost: 5
Attack Speed: 80% of base
Effectiveness of Added Damage: 91%
Experience: 1/70
--------
Requirements:
Level: 1
--------
Performs two fast strikes with a melee weapon.
--------
Deals 91.3% of Base Damage
25% chance to cause Bleeding
Adds 3 to 5 Physical Damage against Bleeding Enemies
--------
Vaal Double Strike
--------
Souls Per Use: 30
Can Store 2 Uses
Soul Gain Prevention: 8 sec
Effectiveness of Added Damage: 28%
--------
Performs two fast strikes with a melee weapon, each of which summons a double of you for a duration to continuously attack monsters in this fashion.
--------
Deals 28% of Base Damage
Base duration is 6.00 seconds
Modifiers to Skill Effect Duration also apply to this Skill's Soul Gain Prevention
Can't be Evaded
25% chance to cause Bleeding
Adds 3 to 5 Physical Damage against Bleeding Enemies
--------
Place into an item socket of the right colour to gain this skill.Right click to remove from a socket.
--------
Corrupted
--------
Note: ~price 2 chaos
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand9 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Divination Card
The Saint's Treasure
--------
Stack Size: 1/10
--------
2x Exalted Orb
--------
Publicly, he lived a pious and chaste life of poverty. Privately, tithes and tributes made him and his lascivious company very comfortable indeed.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        public System.Windows.Input.ICommand DebugPriceCheckCommand10 => new RelayCommand(async _ =>
        {
            var item = await mediator.Send(new ParseItemCommand(@"Rarity: Normal
Contract: Underbelly
--------
Client: Marcine Clavus
Heist Target: Enigmatic Assembly B2 (Moderate Value)
Area Level: 41
Requires Demolition (Level 1)
--------
Item Level: 41
--------
""There is no telling what this device may do when completed, but I
have made it my life's work. There must be a deeper meaning!""
--------
Give this Contract to Adiyah in the Rogue Harbour to embark on the Heist.
"));

            await mediator.Send(new PriceCheckItemCommand(item));
        });

        #endregion
    }
}
