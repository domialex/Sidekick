using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class EquipmentParsing : TestContext<ParserService>
    {
        [Test]
        public async Task ParseUnidentifiedUnique()
        {
            var actual = await Subject.ParseItem(UnidentifiedUnique);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Jade Hatchet");
                actual.Identified.Should().BeFalse();
            }
        }

        [Test]
        public async Task ParseSixLinkUnique()
        {
            var actual = await Subject.ParseItem(UniqueSixLink);

            var expectedAffixes = new[]
            {
                "128% increased Evasion and Energy Shield (Local)",
                "+55 to maximum Life",
                "+12% to all Elemental Resistances",
                "44% increased Area of Effect",
                "47% increased Area Damage",
                "Extra gore"
            };

            var expectedPseudoMods = new[]
            {
                "+12% total to all Elemental Resistances",
                "+36% total Elemental Resistance",
                "+36% total Resistance",
                "+55 total maximum Life"
            };

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Carcass Jack");
                actual.Type.Should().Be("Varnished Coat");

                actual.Properties.Quality.Should().Be(20);
                actual.Properties.Evasion.Should().Be(960);
                actual.Properties.EnergyShield.Should().Be(186);

                actual.Modifiers.Explicit
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedAffixes);

                actual.Modifiers.Pseudo
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedPseudoMods);

                actual.Sockets
                    .Should().HaveCount(6)
                    .And.OnlyContain(socket => socket.Group == 0);
            }
        }

        #region ItemText

        private const string UniqueSixLink = @"Rarity: Unique
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

        private const string UnidentifiedUnique = @"Rarity: Unique
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
        #endregion
    }
}
