using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    public class MapParsing : IClassFixture<SidekickFixture>
    {
        private readonly IMediator mediator;

        public MapParsing(SidekickFixture fixture)
        {
            mediator = fixture.Mediator;
        }

        [Fact]
        public async Task ParseNormalMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(NormalMap));

            Assert.Equal("Beach Map", actual.Type);
            Assert.Equal(1, actual.Properties.MapTier);
        }

        [Fact]
        public async Task ParseMagicMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(MagicMap));

            Assert.Equal("Beach Map", actual.Type);
            Assert.Equal(1, actual.Properties.MapTier);
        }

        [Fact]
        public async Task ParseBlightedMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(BlightedMap));

            Assert.Equal("Ramparts Map", actual.Type);
            Assert.Equal(2, actual.Properties.MapTier);
            Assert.True(actual.Properties.Blighted);
        }

        [Fact]
        public async Task ParseUniqueMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(UniqueMap));

            Assert.Equal("Maelström of Chaos", actual.Name);
            Assert.Equal("Atoll Map", actual.Type);
            Assert.Equal(5, actual.Properties.MapTier);
            Assert.Equal(10, actual.Properties.Quality);
            Assert.Equal(69, actual.Properties.ItemQuantity);
            Assert.Equal(356, actual.Properties.ItemRarity);
        }

        [Fact]
        public async Task ParseOccupiedMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(OccupiedMap));

            Assert.Equal("Carcass Map", actual.Type);
            Assert.Equal(Rarity.Rare, actual.Rarity);
            Assert.Contains("Area is influenced by The Elder", actual.Modifiers.Implicit.Select(x => x.Text));
            Assert.Contains("Map is occupied by The Purifier", actual.Modifiers.Implicit.Select(x => x.Text));
            Assert.Contains("Players are Cursed with Enfeeble, with 60% increased Effect", actual.Modifiers.Explicit.Select(x => x.Text));
        }

        [Fact]
        public async Task ParseTimelessKaruiEmblem()
        {
            var actual = await mediator.Send(new ParseItemCommand(TimelessKaruiEmblem));

            Assert.Equal("Timeless Karui Emblem", actual.Type);
            Assert.Equal(Category.Map, actual.Category);
        }

        #region ItemText

        private const string NormalMap = @"Rarity: Normal
Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
--------
Item Level: 52
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";

        private const string MagicMap = @"Rarity: Magic
Mirrored Beach Map
--------
Map Tier: 1
Atlas Region: Glennach Cairns
Item Quantity: +10% (augmented)
Item Rarity: +6% (augmented)
Monster Pack Size: +4% (augmented)
--------
Item Level: 52
--------
Monsters reflect 13% of Elemental Damage
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
";

        private const string BlightedMap = @"Rarity: Normal
Blighted Ramparts Map
--------
Map Tier: 2
Atlas Region: Glennach Cairns
--------
Item Level: 71
--------
Area is infested with Fungal Growths (implicit)
Natural inhabitants of this area have been removed (implicit)
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.
--------
Note: ~price 33 chaos
";

        private const string UniqueMap = @"Rarity: Unique
Maelström of Chaos
Atoll Map
--------
Map Tier: 5
Atlas Region: Tirn's End
Item Quantity: +69% (augmented)
Item Rarity: +356% (augmented)
Quality: +10% (augmented)
--------
Item Level: 76
--------
Area has patches of chilled ground
Monsters deal 50% extra Damage as Lightning
Monsters are Immune to randomly chosen Elemental Ailments or Stun
Monsters' Melee Attacks apply random Curses on Hit
Monsters reflect Curses
--------
Whispers from a world apart
Speak my name beyond the tomb;
Bound within the Maelström's heart,
Will they grant me strength or doom?
--------
Travel to this Map by using it in a personal Map Device.Maps can only be used once.
";

        private const string OccupiedMap = @"Rarity: Rare
Lost Roost
Carcass Map
--------
Map Tier: 16
Atlas Region: Lex Ejoris
Item Quantity: +91% (augmented)
Item Rarity: +42% (augmented)
Monster Pack Size: +27% (augmented)
Quality: +20% (augmented)
--------
Item Level: 82
--------
Area is influenced by The Elder (implicit)
Map is occupied by The Purifier (implicit)
--------
Players are Cursed with Enfeeble, with 60% increased Effect
Monsters have 70% chance to Avoid Elemental Ailments
Monsters fire 2 additional Projectiles
Monsters' skills Chain 2 additional times
Players gain 50% reduced Flask Charges
--------
Travel to this Map by using it in a personal Map Device. Maps can only be used once.";

        private const string TimelessKaruiEmblem = @"Rarity: Normal
Timeless Karui Emblem
--------
Place two or more different Emblems in a Map Device to access the Domain of Timeless Conflict. Can only be used once.";

        #endregion
    }
}
