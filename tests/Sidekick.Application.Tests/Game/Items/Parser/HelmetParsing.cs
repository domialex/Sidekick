using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class HelmetParsing
    {
        private readonly IMediator mediator;

        public HelmetParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseBlightGuardian()
        {
            var actual = await mediator.Send(new ParseItemCommand(BlightGuardian));

            Assert.Equal(Category.Armour, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Hunter Hood", actual.Metadata.Type);

            var explicits = actual.Modifiers.Explicit.Select(x => x.Text);
            Assert.Contains("You have Shocking Conflux for 3 seconds every 8 seconds", explicits);
        }

        #region ItemText

        private const string BlightGuardian = @"Rarity: Rare
Blight Guardian
Hunter Hood
--------
Evasion Rating: 231 (augmented)
--------
Requirements:
Level: 64
Dex: 87
--------
Sockets: G 
--------
Item Level: 80
--------
Adds 28 to 51 Fire Damage to Spells
+28 to Evasion Rating
+47 to maximum Life
11% increased Rarity of Items found
+29% to Cold Resistance
You have Shocking Conflux for 3 seconds every 8 seconds
--------
Hunter Item
";

        #endregion
    }
}
