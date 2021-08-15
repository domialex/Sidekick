using System.Threading.Tasks;
using Sidekick.Apis.Poe;
using Sidekick.Common.Game.Items;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class DelveParsing
    {
        private readonly IItemParser parser;

        public DelveParsing(ParserFixture fixture)
        {
            parser = fixture.Parser;
        }

        [Fact]
        public async Task ParsePotentChaoticResonator()
        {
            var actual = parser.ParseItem(PotentChaoticResonator);

            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal("Potent Chaotic Resonator", actual.Metadata.Type);
        }

        #region ItemText

        private const string PotentChaoticResonator = @"Item Class: Unknown
Rarity: Currency
Potent Chaotic Resonator
--------
Stack Size: 1/10
Requires 2 Socketed Fossils
--------
Sockets: D D 
--------
Reforges a rare item with new random modifiers
--------
All sockets must be filled with Fossils before this item can be used.
";

        #endregion
    }
}
