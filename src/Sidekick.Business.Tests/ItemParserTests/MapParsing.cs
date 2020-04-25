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
        public async Task ParseNormalMap()
        {
            var actual = await Subject.ParseItem(NormalMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public async Task ParseMagicMap()
        {
            var actual = await Subject.ParseItem(MagicMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public async Task ParseUniqueMap()
        {
            var actual = await Subject.ParseItem(UniqueMap);

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Maelström of Chaos");
                actual.Properties.MapTier.Should().Be(5);
                actual.Properties.Quality.Should().Be(10);
                actual.Properties.ItemQuantity.Should().Be(69);
                actual.Properties.ItemRarity.Should().Be(356);
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

        #endregion
    }
}
