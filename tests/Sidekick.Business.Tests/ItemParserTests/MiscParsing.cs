using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class MiscParsing : TestContext<ParserService>
    {
        [Test]
        public async Task ParseProphecy()
        {
            var actual = await Subject.ParseItem(Prophecy);

            using (new AssertionScope())
            {
                actual.Rarity.Should().Be(Rarity.Prophecy);
                actual.Name.Should().Be("The Four Feral Exiles");
            }
        }

        [Test]
        public async Task ParseDivinationCard()
        {
            var actual = await Subject.ParseItem(DivinationCard);

            using (new AssertionScope())
            {
                actual.Rarity.Should().Be(Rarity.DivinationCard);
                actual.Type.Should().Be("The Saint's Treasure");
            }
        }

        [Test]
        public async Task ParseShaperItemDivinationCard()
        {
            var actual = await Subject.ParseItem(ShaperItemDivinationCard);

            using (new AssertionScope())
            {
                actual.Rarity.Should().Be(Rarity.DivinationCard);
                actual.Type.Should().Be("The Lord of Celebration");
                actual.Influences.Shaper.Should().BeFalse();
            }
        }

        [Test]
        public async Task ParseCurrency()
        {
            var actual = await Subject.ParseItem(Currency);

            using (new AssertionScope())
            {
                actual.Rarity.Should().Be(Rarity.Currency);
                actual.Type.Should().Be("Divine Orb");
            }
        }

        #region ItemText

        private const string Prophecy = @"Rarity: Normal
The Four Feral Exiles
--------
In a faraway dream, four souls far from home prepare to fight to the death.
--------
You will enter a map that holds four additional Rogue Exiles.
--------
Right-click to add this prophecy to your character.
";

        private const string DivinationCard = @"Rarity: Divination Card
The Saint's Treasure
--------
Stack Size: 1/10
--------
2x Exalted Orb
--------
Publicly, he lived a pious and chaste life of poverty. Privately, tithes and tributes made him and his lascivious company very comfortable indeed.
";

        private const string Currency = @"Rarity: Currency
Divine Orb
--------
Stack Size: 2/10
--------
Randomises the numeric values of the random modifiers on an item
--------
Right click this item then left click a magic, rare or unique item to apply it.
Shift click to unstack.
";

        private const string ShaperItemDivinationCard = @"Rarity: Divination Card
The Lord of Celebration
--------
Stack Size: 1/4
--------
Sceptre of Celebration
Shaper Item
--------
Though they were a pack of elite combatants, the Emperor's royal guards were not ready to face one of his notorious parties.";
        #endregion
    }
}
