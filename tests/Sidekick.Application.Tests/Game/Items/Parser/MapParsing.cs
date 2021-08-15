using System.Linq;
using System.Threading.Tasks;
using Sidekick.Apis.Poe;
using Sidekick.Common.Game.Items;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class MapParsing
    {
        private readonly IItemParser parser;

        public MapParsing(ParserFixture fixture)
        {
            parser = fixture.Parser;
        }

        [Fact]
        public async Task ParseNormalMap()
        {
            var actual = parser.ParseItem(NormalMap);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal("Beach Map", actual.Metadata.Type);
            Assert.Equal(1, actual.Properties.MapTier);
        }

        [Fact]
        public async Task ParseMagicMap()
        {
            var actual = parser.ParseItem(MagicMap);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Magic, actual.Metadata.Rarity);
            Assert.Equal("Beach Map", actual.Metadata.Type);
            Assert.Equal(1, actual.Properties.MapTier);
        }

        [Fact]
        public async Task ParseBlightedMap()
        {
            var actual = parser.ParseItem(BlightedMap);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal("Dark Forest Map", actual.Metadata.Type);
            Assert.Equal(14, actual.Properties.MapTier);
            Assert.True(actual.Properties.Blighted);
        }

        [Fact]
        public async Task ParseUniqueMap()
        {
            var actual = parser.ParseItem(UniqueMap);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal("Maelström of Chaos", actual.Metadata.Name);
            Assert.Equal("Atoll Map", actual.Metadata.Type);
            Assert.Equal(5, actual.Properties.MapTier);
            Assert.Equal(10, actual.Properties.Quality);
            Assert.Equal(69, actual.Properties.ItemQuantity);
            Assert.Equal(356, actual.Properties.ItemRarity);
        }

        [Fact]
        public async Task ParseOccupiedMap()
        {
            var actual = parser.ParseItem(OccupiedMap);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Carcass Map", actual.Metadata.Type);

            var implicits = actual.Modifiers.Implicit.Select(x => x.Text);
            Assert.Contains("Area is influenced by The Elder", implicits);
            Assert.Equal(2, actual.Modifiers.Implicit.First(x => x.Text.Contains("The Elder")).OptionValue.Value);
            Assert.Contains("Map is occupied by The Purifier", implicits);
            Assert.Contains("Players are Cursed with Enfeeble, with 60% increased Effect", actual.Modifiers.Explicit.Select(x => x.Text));
        }

        [Fact]
        public async Task ParseTimelessKaruiEmblem()
        {
            var actual = parser.ParseItem(TimelessKaruiEmblem);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal("Timeless Karui Emblem", actual.Metadata.Type);
        }

        [Fact]
        public async Task ParseVortexPit()
        {
            var actual = parser.ParseItem(VortexPit);

            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Burial Chambers Map", actual.Metadata.Type);

            var explicits = actual.Modifiers.Explicit.Select(x => x.Text);
            Assert.DoesNotContain("#% increased Area of Effect", explicits);
        }

        #region ItemText

        private const string NormalMap = @"Item Class: Maps
Rarity: Normal
Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
--------
Item Level: 52
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";

        private const string MagicMap = @"Item Class: Maps
Rarity: Magic
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

        private const string BlightedMap = @"Item Class: Maps
Rarity: Normal
Blighted Dark Forest Map
--------
Map Tier: 14
--------
Item Level: 83
--------
Area is infested with Fungal Growths
Map's Item Quantity Modifiers also affect Blight Chest count at 20% value (implicit)
Natural inhabitants of this area have been removed (implicit)
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
";

        private const string UniqueMap = @"Item Class: Maps
Rarity: Unique
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

        private const string OccupiedMap = @"Item Class: Maps
Rarity: Rare
Lost Roost
Carcass Map
--------
Map Tier: 16
Atlas Region: Lex Ejoris
Item Quantity: +91% (augmented)
Item Rarity: +42% (augmented)
Monster Pack Size: +27% (augmented)
Quality: +20% (augmented)
--------
Item Level: 82
--------
Area is influenced by The Elder (implicit)
Map is occupied by The Purifier (implicit)
--------
Players are Cursed with Enfeeble, with 60% increased Effect
Monsters have 70% chance to Avoid Elemental Ailments
Monsters fire 2 additional Projectiles
Monsters' skills Chain 2 additional times
Players gain 50% reduced Flask Charges
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.";

        private const string TimelessKaruiEmblem = @"Item Class: Map Fragments
Rarity: Normal
Timeless Karui Emblem
--------
Place two or more different Emblems in a Map Device to access the Domain of Timeless Conflict. Can only be used once.
";

        private const string VortexPit = @"Item Class: Unknown
Rarity: Rare
Vortex Pit
Burial Chambers Map
--------
Map Tier: 16
Atlas Region: Lex Ejoris
Item Quantity: +70% (augmented)
Item Rarity: +31% (augmented)
Monster Pack Size: +20% (augmented)
Quality: +18% (augmented)
--------
Item Level: 82
--------
Area is influenced by The Elder (implicit)
Map is occupied by The Eradicator (implicit)
--------
29% more Magic Monsters
22% more Rare Monsters
Monsters have 100% increased Area of Effect
Monsters Poison on Hit
Rare Monsters each have a Nemesis Mod
Magic Monster Packs each have a Bloodline Mod
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
";

        #endregion
    }
}
