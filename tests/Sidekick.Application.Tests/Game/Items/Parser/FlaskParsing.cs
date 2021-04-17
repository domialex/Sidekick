using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class FlaskParsing
    {
        private readonly IMediator mediator;

        public FlaskParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseSanctifiedManaFlask()
        {
            var actual = await mediator.Send(new ParseItemCommand(SanctifiedManaFlask));

            Assert.Equal(Category.Flask, actual.Metadata.Category);
            Assert.Equal(Rarity.Magic, actual.Metadata.Rarity);
            Assert.Equal("Sanctified Mana Flask", actual.Metadata.Type);

            var explicits = actual.Modifiers.Explicit.Select(x => x.Text);
            Assert.Contains("Immunity to Bleeding during Flask effect\nRemoves Bleeding on use", explicits);
        }

        #region ItemText

        private const string SanctifiedManaFlask = @"Item Class: Unknown
Rarity: Magic
Concentrated Sanctified Mana Flask of Staunching
--------
Recovers 1430 (augmented) Mana over 6.50 Seconds
Consumes 8 (augmented) of 35 Charges on use
Currently has 0 Charges
--------
Requirements:
Level: 50
--------
Item Level: 52
--------
30% increased Amount Recovered
22% increased Charges used
Immunity to Bleeding during Flask effect
Removes Bleeding on use
--------
Right click to drink. Can only hold charges while in belt. Refills as you kill monsters.
--------
Note: ~price 1 chance
";

        #endregion
    }
}
