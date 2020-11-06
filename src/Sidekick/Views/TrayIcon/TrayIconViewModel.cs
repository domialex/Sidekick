using System;
using System.Windows.Input;
using Sidekick.Domain.Clipboard;
using Sidekick.Presentation.Views;

namespace Sidekick.Views.TrayIcon
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

        public ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open(View.Settings));

        public ICommand ShowAboutCommand => new RelayCommand(_ => viewLocator.Open(View.About));

        public ICommand ShowLogsCommand => new RelayCommand(_ => viewLocator.Open(View.Logs));

        public ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());

        public ICommand DebugLeagueOverlayCommand => new RelayCommand(_ => viewLocator.Open(View.League));

        public ICommand DebugCrashCommand => new RelayCommand(_ => throw new Exception("Crash requested via tray icon"));

        #region Debug Price Check
        public ICommand DebugPriceCheckCommand0 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(await nativeClipboard.GetText());

            viewLocator.Open(View.Price);
        });

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

            viewLocator.Open(View.Price);
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

            viewLocator.Open(View.Price);
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

            viewLocator.Open(View.Price);
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

            viewLocator.Open(View.Price);
        });

        public ICommand DebugPriceCheckCommand5 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rareté: Unique
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
");

            viewLocator.Open(View.Price);
        });

        public ICommand DebugPriceCheckCommand6 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Rare
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
");

            viewLocator.Open(View.Price);
        });

        public ICommand DebugPriceCheckCommand7 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Rare
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
");

            viewLocator.Open(View.Price);
        });

        public ICommand DebugPriceCheckCommand8 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Gem
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
");

            viewLocator.Open(View.Price);
        });

        public ICommand DebugPriceCheckCommand9 => new RelayCommand(async _ =>
        {
            await nativeClipboard.SetText(@"Rarity: Divination Card
The Saint's Treasure
--------
Stack Size: 1/10
--------
2x Exalted Orb
--------
Publicly, he lived a pious and chaste life of poverty. Privately, tithes and tributes made him and his lascivious company very comfortable indeed.
");

            viewLocator.Open(View.Price);
        });

        #endregion
    }
}
