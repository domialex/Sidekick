using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class MiscParsing
    {
        private readonly IMediator mediator;

        public MiscParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseProphecy()
        {
            var actual = await mediator.Send(new ParseItemCommand(Prophecy));

            Assert.Equal(Category.Prophecy, actual.Category);
            Assert.Equal(Rarity.Prophecy, actual.Rarity);
            Assert.Equal("The Four Feral Exiles", actual.Name);
        }

        [Fact]
        public async Task ParseDivinationCard()
        {
            var actual = await mediator.Send(new ParseItemCommand(DivinationCard));

            Assert.Equal(Category.DivinationCard, actual.Category);
            Assert.Equal(Rarity.DivinationCard, actual.Rarity);
            Assert.Equal("The Saint's Treasure", actual.Name);
        }

        [Fact]
        public async Task ParseShaperItemDivinationCard()
        {
            var actual = await mediator.Send(new ParseItemCommand(ShaperItemDivinationCard));

            Assert.Equal(Category.DivinationCard, actual.Category);
            Assert.Equal(Rarity.DivinationCard, actual.Rarity);
            Assert.Equal("The Lord of Celebration", actual.Name);
            Assert.False(actual.Influences.Shaper);
        }

        [Fact]
        public async Task ParseCurrency()
        {
            var actual = await mediator.Send(new ParseItemCommand(Currency));

            Assert.Equal(Category.Currency, actual.Category);
            Assert.Equal(Rarity.Currency, actual.Rarity);
            Assert.Equal("Divine Orb", actual.Name);
        }

        [Fact]
        public async Task ParseOrgan()
        {
            var actual = await mediator.Send(new ParseItemCommand(Organ));

            Assert.Equal(Category.ItemisedMonster, actual.Category);
            Assert.Equal(Rarity.Unique, actual.Rarity);
            Assert.Equal("Portentia, the Foul", actual.Name);
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

        private const string Organ = @"Rarity: Unique
Portentia, the Foul's Heart
--------
Uses: Blood Bubble
--------
Item Level: 79
--------
Drops a Rare Weapon
Drops additional Rare Armour
Drops additional Rare Armour
Drops additional Rare Jewellery
--------
Combine this with four other different samples in Tane's Laboratory.";
        #endregion
    }
}
