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
        public async Task ParseHypnoticCharm()
        {
            var actual = await mediator.Send(new ParseItemCommand(HypnoticCharm));

            Assert.Equal(Category.Weapon, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Imbued Wand", actual.Metadata.Type);

            var crafteds = actual.Modifiers.Crafted.Select(x => x.Text);
            Assert.Contains("Trigger a Socketed Spell when you Use a Skill, with a 8 second Cooldown", crafteds);
        }

        #region ItemText

        private const string HypnoticCharm = @"Item Class: Wands
Rarity: Rare
Hypnotic Charm
Imbued Wand
--------
Wand
Quality: +20% (augmented)
Physical Damage: 28-53
Critical Strike Chance: 7.00%
Attacks per Second: 1.50
--------
Requirements:
Level: 60
Int: 188
--------
Sockets: B-B-B 
--------
Item Level: 69
--------
Quality does not increase Physical Damage (enchant)
Grants 1% increased Elemental Damage per 2% Quality (enchant)
--------
34% increased Spell Damage (implicit)
--------
61% increased Spell Damage
29% increased Critical Strike Chance for Spells
+23 to maximum Mana
+1 to Level of all Fire Spell Skill Gems
26% increased Burning Damage
Trigger a Socketed Spell when you Use a Skill, with a 8 second Cooldown (crafted)
";

        #endregion
    }
}
