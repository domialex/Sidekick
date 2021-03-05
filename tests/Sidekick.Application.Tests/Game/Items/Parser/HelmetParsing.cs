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

        [Fact]
        public async Task ParseStarkonjaHead()
        {
            var actual = await mediator.Send(new ParseItemCommand(StarkonjaHead));

            Assert.Equal(Category.Armour, actual.Metadata.Category);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal("Starkonja's Head", actual.Metadata.Name);
            Assert.Equal("Silken Hood", actual.Metadata.Type);

            var enchants = actual.Modifiers.Enchant.Select(x => x.Text);
            Assert.Contains("Divine Ire Damages 2 additional nearby Enemies when gaining Stages", enchants);
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

        private const string StarkonjaHead = @"Rarity: Unique
Starkonja's Head
Silken Hood
--------
Evasion Rating: 765 (augmented)
--------
Requirements:
Level: 60
Dex: 138
--------
Sockets: G G 
--------
Item Level: 80
--------
Divine Ire Damages 2 additional nearby Enemies when gaining Stages (enchant)
--------
+57 to Dexterity
50% reduced Damage when on Low Life
10% increased Attack Speed
25% increased Global Critical Strike Chance
121% increased Evasion Rating
+87 to maximum Life
150% increased Global Evasion Rating when on Low Life
--------
There was no hero made out of Starkonja's death,
but merely a long sleep made eternal.
";

        #endregion
    }
}
