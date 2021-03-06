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
            Assert.Equal("Portentia, the Foul's Heart", actual.Metadata.Name);
            Assert.Equal("Portentia, the Foul", actual.Metadata.Type);
        }

        [Fact]
        public async Task ParseRareBeast()
        {
            var parsedRareBeast = await mediator.Send(new ParseItemCommand(RareBeast));

            Assert.Equal(Category.ItemisedMonster, parsedRareBeast.Metadata.Category);
            Assert.Equal(Rarity.Rare, parsedRareBeast.Metadata.Rarity);
            Assert.Null(parsedRareBeast.Metadata.Name);
            Assert.Equal("Farric Tiger Alpha", parsedRareBeast.Metadata.Type);
        }

        [Fact]
        public async Task ParseUniqueBeast()
        {
            var parsedRareBeast = await mediator.Send(new ParseItemCommand(UniqueBeast));

            Assert.Equal(Category.ItemisedMonster, parsedRareBeast.Metadata.Category);
            Assert.Equal(Rarity.Unique, parsedRareBeast.Metadata.Rarity);
            Assert.Equal("Saqawal, First of the Sky", parsedRareBeast.Metadata.Name);
            Assert.Equal("Saqawal, First of the Sky", parsedRareBeast.Metadata.Type);
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

        private const string Organ = @"Item Class: Metamorph Sample
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

        private const string RareBeast = @"Item Class: Stackable Currency
Rarity: Rare
Foulface the Ravager
Farric Tiger Alpha
--------
Genus: Tigers
Group: Felines
Family: The Wilds
--------
Item Level: 82
--------
Tiger Prey
Spectral Swipe
Accurate
Evasive
Gains Endurance Charges
--------
Right-click to add this to your bestiary.";

        private const string UniqueBeast = @"Item Class: Stackable Currency
Rarity: Unique
Saqawal, First of the Sky
--------
Genus: Rhexes
Group: Avians
Family: The Sands
--------
Item Level: 70
--------
Cannot be fully Slowed
--------
Right-click to add this to your bestiary.";
        #endregion
    }
}
