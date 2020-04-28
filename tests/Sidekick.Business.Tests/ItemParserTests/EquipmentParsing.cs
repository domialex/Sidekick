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
        public async Task ParseSixLinkUniqueBodyArmor()
        {
            var actual = await Subject.ParseItem(UniqueSixLink);

            var expectedExplicits = new[]
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
                    .Should().Contain(expectedExplicits);

                actual.Modifiers.Pseudo
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedPseudoMods);

                actual.Sockets
                    .Should().HaveCount(6)
                    .And.OnlyContain(socket => socket.Group == 0);
            }
        }

        [Test]
        public async Task ParseRareGloves()
        {
            var actual = await Subject.ParseItem(GlovesAssasinsMitts);

            var expectedExplicits = new[]
            {
                "+18 to Intelligence",
                "+73 to maximum Life",
                "+14% to Lightning Resistance",
                "0.23% of Physical Attack Damage Leeched as Mana"
            };

            using (new AssertionScope())
            {
                actual.NameLine.Should().Be("Death Nails");
                actual.Type.Should().Be("Assassin's Mitts");

                actual.Modifiers.Explicit
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedExplicits);

                actual.Sockets.Count.Should().Be(1);
            }
        }

        [Test]
        public async Task ParseJewel()
        {
            var actual = await Subject.ParseItem(JewelBlightCut);

            var expectedExplicits = new[]
            {
                "+8 to Strength and Intelligence",
                "14% increased Spell Damage while Dual Wielding",
                "19% increased Burning Damage",
                "15% increased Damage with Wands"
            };

            using (new AssertionScope())
            {
                actual.NameLine.Should().Be("Blight Cut");
                actual.Type.Should().Be("Cobalt Jewel");

                actual.Modifiers.Explicit
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedExplicits);

                actual.ItemLevel.Should().Be(68);
            }
        }

        [Test]
        public async Task ParseInfluencedWeapon()
        {
            var actual = await Subject.ParseItem(InfluencedWand);

            var expectedExplicits = new[]
            {
                "Adds 10 to 16 Physical Damage",
                "24% increased Fire Damage",
                "14% increased Critical Strike Chance for Spells",
                "Attacks with this Weapon Penetrate 10% Lightning Resistance"
            };

            using (new AssertionScope())
            {
                actual.NameLine.Should().Be("Miracle Chant");
                actual.Type.Should().Be("Imbued Wand");

                actual.Modifiers.Explicit
                    .Select(mod => mod.Text)
                    .Should().Contain(expectedExplicits);

                actual.Modifiers.Implicit
                    .Should().ContainSingle(mod => mod.Text == "33% increased Spell Damage");

                actual.Influences.Crusader.Should().BeTrue();
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

        private const string GlovesAssasinsMitts = @"Rarity: Rare
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
";

        private const string JewelBlightCut = @"Rarity: Rare
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
";

        private const string InfluencedWand = @"Rarity: Rare
Miracle Chant
Imbued Wand
--------
Wand
Physical Damage: 38-69 (augmented)
Critical Strike Chance: 7.00%
Attacks per Second: 1.50
--------
Requirements:
Level: 59
Int: 188
--------
Sockets: R B 
--------
Item Level: 70
--------
33% increased Spell Damage (implicit)
--------
Adds 10 to 16 Physical Damage
24% increased Fire Damage
14% increased Critical Strike Chance for Spells
Attacks with this Weapon Penetrate 10% Lightning Resistance
--------
Crusader Item
";

        #endregion
    }
}
