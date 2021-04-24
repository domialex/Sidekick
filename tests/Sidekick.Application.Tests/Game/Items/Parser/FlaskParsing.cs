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
            Assert.Contains("Immunity to Bleeding and Corrupted Blood during Flask effect\nRemoves Bleeding and Corrupted Blood on use", explicits);
        }

        #region ItemText

        private const string SanctifiedManaFlask = @"Item Class: Mana Flasks
Rarity: Magic
Sanctified Mana Flask of Staunching
--------
Recovers 1100 Mana over 6.50 Seconds
Consumes 7 of 35 Charges on use
Currently has 0 Charges
--------
Requirements:
Level: 50
--------
Item Level: 60
--------
Immunity to Bleeding and Corrupted Blood during Flask effect
Removes Bleeding and Corrupted Blood on use
--------
Right click to drink. Can only hold charges while in belt. Refills as you kill monsters.
";

        #endregion
    }
}
