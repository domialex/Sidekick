using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class RingParsing
    {
        private readonly IMediator mediator;

        public RingParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseAgonyFinger()
        {
            var actual = await mediator.Send(new ParseItemCommand(AgonyFinger));

            Assert.Equal(Category.Accessory, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Amethyst Ring", actual.Metadata.Type);

            var implicits = actual.Modifiers.Implicit.Select(x => x.Text);
            Assert.Contains("Cannot be Ignited", implicits);
            Assert.Contains("Anger has 20% increased Aura Effect", implicits);
        }

        #region ItemText

        private const string AgonyFinger = @"Rarity: Rare
Agony Finger
Amethyst Ring
--------
Requirements:
Level: 59
--------
Item Level: 77
--------
Cannot be Ignited (implicit)
Anger has 20% increased Aura Effect (implicit)
--------
+49 to Dexterity
+78 to Evasion Rating
+35 to maximum Mana
+31% to Lightning Resistance
+7% to Chaos Resistance
0.39% of Physical Attack Damage Leeched as Life
--------
Corrupted
";

        #endregion
    }
}
