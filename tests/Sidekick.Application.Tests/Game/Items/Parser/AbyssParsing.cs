using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class AbyssParsing
    {
        private readonly IMediator mediator;

        public AbyssParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseBulbonicTrail()
        {
            var actual = await mediator.Send(new ParseItemCommand(BulbonicTrail));

            Assert.Equal(Category.Armour, actual.Metadata.Category);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal("Bubonic Trail", actual.Metadata.Name);
            Assert.Equal("Murder Boots", actual.Metadata.Type);

            var explicits = actual.Modifiers.Explicit.Select(x => x.Text);
            Assert.Contains("Has 1 Abyssal Socket", explicits);
        }

        #region ItemText

        private const string BulbonicTrail = @"Item Class: Unknown
Rarity: Unique
Bubonic Trail
Murder Boots
--------
Evasion Rating: 185
Energy Shield: 17
--------
Requirements:
Level: 69
Dex: 82
Int: 42
--------
Sockets: G A 
--------
Item Level: 76
--------
Has 1 Abyssal Socket
Triggers Level 20 Death Walk when Equipped
4% increased maximum Life
30% increased Movement Speed
10% increased Damage for each type of Abyss Jewel affecting you
--------
Even the dead serve the Lightless.
";

        #endregion
    }
}
