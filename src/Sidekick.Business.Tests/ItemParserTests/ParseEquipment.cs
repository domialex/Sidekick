using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class ParseEquipment : TestContext<ParserService>
    {
        [Test]
        public async Task ParseUnidentifiedUnique()
        {
            var actual = await Subject.ParseItem(ExampleItems.UnidentifiedUnique);

            using (new AssertionScope())
            {
                actual.Type.Should().Be("Jade Hatchet");
                actual.Identified.Should().BeFalse();
            }
        }

        [Test]
        public async Task ParseSixLinkUnique()
        {
            var actual = await Subject.ParseItem(ExampleItems.UniqueSixLink);

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Carcass Jack");
                actual.Type.Should().Be("Varnished Coat");
                actual.Sockets
                    .Should().HaveCount(6)
                    .And.OnlyContain(socket => socket.Group == 0);
            }
        }
    }
}
