using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public static class ExampleItems
    {
        public const string NormalMap = @"Rarity: Normal
Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
--------
Item Level: 52
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";

        public const string MagicMap = @"Rarity: Magic
Mirrored Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
Item Quantity: +10% (augmented)
Item Rarity: +6% (augmented)
Monster Pack Size: +4% (augmented)
--------
Item Level: 52
--------
Monsters reflect 13% of Elemental Damage
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
";

        public const string UniqueMap = @"Rarity: Unique
Maelström of Chaos
Atoll Map
--------
Map Tier: 5
Atlas Region: Tirn's End
Item Quantity: +69% (augmented)
Item Rarity: +356% (augmented)
Quality: +10% (augmented)
--------
Item Level: 76
--------
Area has patches of chilled ground
Monsters deal 50% extra Damage as Lightning
Monsters are Immune to randomly chosen Elemental Ailments or Stun
Monsters' Melee Attacks apply random Curses on Hit
Monsters reflect Curses
--------
Whispers from a world apart
Speak my name beyond the tomb;
Bound within the Maelström's heart,
Will they grant me strength or doom?
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";


        public const string UniqueSixLink = @"Rarity: Unique
Carcass Jack
Varnished Coat
--------
Quality: +20% (augmented)
Evasion Rating: 960 (augmented)
Energy Shield: 186 (augmented)
--------
Requirements:
Level: 70
Str: 68
Dex: 96
Int: 111
--------
Sockets: B-B-R-B-B-B 
--------
Item Level: 81
--------
128% increased Evasion and Energy Shield
+55 to maximum Life
+12% to all Elemental Resistances
44% increased Area of Effect
47% increased Area Damage
Extra gore
--------
""...The discomfort shown by the others is amusing, but none
can deny that my work has made quite the splash...""
- Maligaro's Journal
";

        public const string UnidentifiedUnique = @"Rarity: Unique
Jade Hatchet
--------
One Handed Axe
Physical Damage: 10-15
Critical Strike Chance: 5.00%
Attacks per Second: 1.45
Weapon Range: 11
--------
Requirements:
Str: 21
--------
Sockets: R-G-B 
--------
Item Level: 71
--------
Unidentified
;";
    }
}
