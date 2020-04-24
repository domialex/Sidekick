using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class ParseMaps : TestContext<ParserService>
    {
        [Test]
        public async Task ParseNormalMap()
        {
            var actual = await Subject.ParseItem(ExampleItems.NormalMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public async Task ParseMagicMap()
        {
            var actual = await Subject.ParseItem(ExampleItems.MagicMap);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Beach Map");
                actual.Properties.MapTier.Should().Be(1);
            }
        }

        [Test]
        public async Task ParseUniqueMap()
        {
            var actual = await Subject.ParseItem(ExampleItems.UniqueMap);

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Maelström of Chaos");
                actual.Properties.MapTier.Should().Be(5);
                actual.Properties.Quality.Should().Be(10);
                actual.Properties.ItemQuantity.Should().Be(69);
                actual.Properties.ItemRarity.Should().Be(356);
            }
        }
    }
}
