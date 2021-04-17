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

            Assert.Equal(Category.Prophecy, actual.Metadata.Category);
            Assert.Equal(Rarity.Prophecy, actual.Metadata.Rarity);
            Assert.Equal("The Four Feral Exiles", actual.Metadata.Name);
            Assert.Equal("Prophecy", actual.Metadata.Type);
        }

        [Fact]
        public async Task ParseCurrency()
        {
            var actual = await mediator.Send(new ParseItemCommand(Currency));

            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Null(actual.Metadata.Name);
            Assert.Equal("Divine Orb", actual.Metadata.Type);
        }

        [Fact]
        public async Task ParseOrgan()
        {
            var actual = await mediator.Send(new ParseItemCommand(Organ));

            Assert.Equal(Category.ItemisedMonster, actual.Metadata.Category);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Null(actual.Metadata.Name);
            Assert.Equal("Portentia, the Foul", actual.Metadata.Type);
        }

        #region ItemText

        private const string Prophecy = @"Item Class: Unknown
Rarity: Normal
The Four Feral Exiles
--------
In a faraway dream, four souls far from home prepare to fight to the death.
--------
You will enter a map that holds four additional Rogue Exiles.
--------
Right-click to add this prophecy to your character.
";

        private const string Currency = @"Item Class: Unknown
Rarity: Currency
Divine Orb
--------
Stack Size: 2/10
--------
Randomises the numeric values of the random modifiers on an item
--------
Right click this item then left click a magic, rare or unique item to apply it.
Shift click to unstack.
";

        private const string Organ = @"Item Class: Unknown
Rarity: Unique
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
