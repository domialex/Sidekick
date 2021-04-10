using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class DivinationCardParsing
    {
        private readonly IMediator mediator;

        public DivinationCardParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseSaintTreasure()
        {
            var actual = await mediator.Send(new ParseItemCommand(SaintTreasure));

            Assert.Equal(Category.DivinationCard, actual.Metadata.Category);
            Assert.Equal(Rarity.DivinationCard, actual.Metadata.Rarity);
            Assert.Null(actual.Metadata.Name);
            Assert.Equal("The Saint's Treasure", actual.Metadata.Type);
        }

        [Fact]
        public async Task ParseLordOfCelebration()
        {
            var actual = await mediator.Send(new ParseItemCommand(LordOfCelebration));

            Assert.Equal(Category.DivinationCard, actual.Metadata.Category);
            Assert.Equal(Rarity.DivinationCard, actual.Metadata.Rarity);
            Assert.Null(actual.Metadata.Name);
            Assert.Equal("The Lord of Celebration", actual.Metadata.Type);
            Assert.False(actual.Influences.Crusader);
            Assert.False(actual.Influences.Elder);
            Assert.False(actual.Influences.Hunter);
            Assert.False(actual.Influences.Redeemer);
            Assert.False(actual.Influences.Shaper);
            Assert.False(actual.Influences.Warlord);
        }

        #region ItemText

        private const string SaintTreasure = @"Rarity: Divination Card
The Saint's Treasure
--------
Stack Size: 1/10
--------
2x Exalted Orb
--------
Publicly, he lived a pious and chaste life of poverty. Privately, tithes and tributes made him and his lascivious company very comfortable indeed.
";

        private const string LordOfCelebration = @"Rarity: Divination Card
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
