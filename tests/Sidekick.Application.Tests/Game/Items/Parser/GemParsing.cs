using System.Threading.Tasks;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    public class GemParsing : IClassFixture<SidekickFixture>
    {
        private readonly SidekickFixture fixture;

        public GemParsing(SidekickFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task ParseVaalGem()
        {
            var actual = await fixture.Mediator.Send(new ParseItemCommand(@"Rarity: Gem
Double Strike
--------
Vaal, Attack, Melee, Strike, Duration, Physical
Level: 1
Mana Cost: 5
Attack Speed: 80% of base
Effectiveness of Added Damage: 91%
Experience: 1/70
--------
Requirements:
Level: 1
--------
Performs two fast strikes with a melee weapon.
--------
Deals 91.3% of Base Damage
25% chance to cause Bleeding
Adds 3 to 5 Physical Damage against Bleeding Enemies
--------
Vaal Double Strike
--------
Souls Per Use: 30
Can Store 2 Uses
Soul Gain Prevention: 8 sec
Effectiveness of Added Damage: 28%
--------
Performs two fast strikes with a melee weapon, each of which summons a double of you for a duration to continuously attack monsters in this fashion.
--------
Deals 28% of Base Damage
Base duration is 6.00 seconds
Modifiers to Skill Effect Duration also apply to this Skill's Soul Gain Prevention
Can't be Evaded
25% chance to cause Bleeding
Adds 3 to 5 Physical Damage against Bleeding Enemies
--------
Place into an item socket of the right colour to gain this skill.Right click to remove from a socket.
--------
Corrupted
--------
Note: ~price 2 chaos
"));

            Assert.Equal(Rarity.Gem, actual.Rarity);
            Assert.Equal("Vaal Double Strike", actual.Type);
            Assert.Equal(1, actual.Properties.GemLevel);
            Assert.True(actual.Corrupted);
        }

    }
}
