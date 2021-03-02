using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class JewelParsing
    {
        private readonly IMediator mediator;

        public JewelParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseJewelBlightCut()
        {
            var actual = await mediator.Send(new ParseItemCommand(JewelBlightCut));

            Assert.Equal(Category.Jewel, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Cobalt Jewel", actual.Metadata.Type);
            Assert.Equal("Blight Cut", actual.Original.Name);
            Assert.Equal(68, actual.Properties.ItemLevel);

            var explicits = actual.Modifiers.Explicit.Select(x => x.Text);
            Assert.Contains("+8 to Strength and Intelligence", explicits);
            Assert.Contains("14% increased Spell Damage while Dual Wielding", explicits);
            Assert.Contains("19% increased Burning Damage", explicits);
            Assert.Contains("15% increased Damage with Wands", explicits);
        }

        [Fact]
        public async Task ParseJewelLoathHope()
        {
            var actual = await mediator.Send(new ParseItemCommand(JewelLoathHope));

            Assert.Equal(Category.Jewel, actual.Metadata.Category);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal("Large Cluster Jewel", actual.Metadata.Type);
            Assert.Equal("Loath Hope", actual.Original.Name);
            Assert.Equal(69, actual.Properties.ItemLevel);

            var enchants = actual.Modifiers.Enchant.Select(x => x.Text);
            Assert.Contains("Added Small Passive Skills grant: Axe Attacks deal 12% increased Damage with Hits and Ailments\nAdded Small Passive Skills grant: Sword Attacks deal 12% increased Damage with Hits and Ailments", enchants);
        }

        #region ItemText
        private const string JewelBlightCut = @"Rarity: Rare
Blight Cut
Cobalt Jewel
--------
Item Level: 68
--------
+8 to Strength and Intelligence
14% increased Spell Damage while Dual Wielding
19% increased Burning Damage
15% increased Damage with Wands
--------
Place into an allocated Jewel Socket on the Passive Skill Tree.Right click to remove from the Socket.
";

        private const string JewelLoathHope = @"Rarity: Rare
Loath Hope
Large Cluster Jewel
--------
Requirements:
Level: 54
--------
Item Level: 69
--------
Adds 11 Passive Skills (enchant)
2 Added Passive Skills are Jewel Sockets (enchant)
Added Small Passive Skills grant: Axe Attacks deal 12% increased Damage with Hits and Ailments
Added Small Passive Skills grant: Sword Attacks deal 12% increased Damage with Hits and Ailments (enchant)
--------
Added Small Passive Skills also grant: +7 to Maximum Energy Shield
Added Small Passive Skills also grant: +5 to Strength
Added Small Passive Skills have 25% increased Effect
1 Added Passive Skill is Heavy Hitter
--------
Place into an allocated Large Jewel Socket on the Passive Skill Tree. Added passives do not interact with jewel radiuses. Right click to remove from the Socket.
--------
Note: ~price 1 alch
";
        #endregion
    }
}
