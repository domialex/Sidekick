using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class DelveParsing
    {
        private readonly IMediator mediator;

        public DelveParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseResonator()
        {
            var actual = await mediator.Send(new ParseItemCommand(PotentChaoticResonator));

            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal("Potent Chaotic Resonator", actual.Metadata.Type);
        }

        #region ItemText

        private const string PotentChaoticResonator = @"Rarity: Currency
Potent Chaotic Resonator
--------
Stack Size: 1/10
Requires 2 Socketed Fossils
--------
Sockets: D D 
--------
Reforges a rare item with new random modifiers
--------
All sockets must be filled with Fossils before this item can be used.
";

        #endregion
    }
}
