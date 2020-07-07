using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class MapParsing : TestContext<ParserService>
    {
        [Test]
        public void ParseNormalMap()
        {
            var actual = Subject.ParseItem(NormalMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public void ParseMagicMap()
        {
            var actual = Subject.ParseItem(MagicMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public void ParseBlightedMap()
        {
            var actual = Subject.ParseItem(BlightedMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Ramparts Map");
                actual.Properties.MapTier.Should().Be(2);
                actual.Properties.Blighted.Should().BeTrue();
            }
        }

        [Test]
        public void ParseUniqueMap()
        {
            var actual = Subject.ParseItem(UniqueMap);

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Maelström of Chaos");
                actual.Properties.MapTier.Should().Be(5);
                actual.Properties.Quality.Should().Be(10);
                actual.Properties.ItemQuantity.Should().Be(69);
                actual.Properties.ItemRarity.Should().Be(356);
            }
        }

        [Test]
        public void ParseOccupiedMap()
        {
            var actual = Subject.ParseItem(OccupiedMap);

            using (new AssertionScope())
            {
                var expectedImplicits = new[]
                {
                    "Area is influenced by The Elder",
                    "Map is occupied by The Purifier"
                };

                var expectedExplicits = new[]
                {
                    "Players are Cursed with Enfeeble"
                };

                actual.Type.Should().Be("Carcass Map");
                actual.Rarity.Should().Be(Apis.Poe.Models.Rarity.Rare);
                actual.Modifiers.Implicit
                      .Select(mod => mod.Text)
                      .Should().Contain(expectedImplicits);
                actual.Modifiers.Explicit
                      .Select(mod => mod.Text)
                      .Should().Contain(expectedExplicits);
            }
        }

        #region ItemText

        private const string NormalMap = @"Rarity: Normal
Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
--------
Item Level: 52
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";

        private const string MagicMap = @"Rarity: Magic
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

        private const string BlightedMap = @"Rarity: Normal
Blighted Ramparts Map
--------
Map Tier: 2
Atlas Region: Glennach Cairns
--------
Item Level: 71
--------
Area is infested with Fungal Growths (implicit)
Natural inhabitants of this area have been removed (implicit)
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
--------
Note: ~price 33 chaos
";

        private const string UniqueMap = @"Rarity: Unique
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

        private const string OccupiedMap = @"Rarity: Rare
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
Players are Cursed with Enfeeble
Monsters have 70% chance to Avoid Elemental Ailments
Monsters fire 2 additional Projectiles
Monsters' skills Chain 2 additional times
Players gain 50% reduced Flask Charges
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.";

        #endregion
    }
}
