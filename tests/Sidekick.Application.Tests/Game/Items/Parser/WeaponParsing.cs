using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class WeaponParsing
    {
        private readonly IMediator mediator;

        public WeaponParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseVortexWeaver()
        {
            var actual = await mediator.Send(new ParseItemCommand(VortexWeaver));

            Assert.Equal(Category.Weapon, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Vaal Sceptre", actual.Metadata.Type);

            var crafteds = actual.Modifiers.Crafted.Select(x => x.Text);
            Assert.Contains("Trigger a Socketed Spell when you Use a Skill", crafteds);
        }

        #region ItemText

        private const string VortexWeaver = @"Item Class: Unknown
Rarity: Rare
Vortex Weaver
Vaal Sceptre
--------
Sceptre
Physical Damage: 37-70
Elemental Damage: 2-43 (augmented)
Critical Strike Chance: 6.00%
Attacks per Second: 1.40
Weapon Range: 11
--------
Requirements:
Level: 64
Str: 113
Int: 113
--------
Sockets: R-B 
--------
Item Level: 80
--------
32% increased Elemental Damage (implicit)
--------
Adds 2 to 43 Lightning Damage
Adds 14 to 27 Fire Damage to Spells
5% increased Cast Speed
+5 Mana gained on Kill
Trigger a Socketed Spell when you Use a Skill (crafted)
";

        #endregion
    }
}
