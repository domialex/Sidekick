using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using Sidekick.Business.Parsers;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Tests.ItemParserTests
{
    public class ParseEquipment : TestContext<ItemParser>
    {
        [Test]
        public async Task ParseUnidentifiedUnique()
        {
            var actual = await Subject.ParseItem(ExampleItems.UnidentifiedUnique) as EquippableItem;

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Jade Hatchet");
                actual.Type.Should().Be("Jade Hatchet");
                actual.IsIdentified.Should().BeFalse();
            }
        }

        [Test]
        public async Task ParseSixLinkUnique()
        {
            var actual = await Subject.ParseItem(ExampleItems.UniqueSixLink) as EquippableItem;

            using (new AssertionScope())
            {
                actual.Name.Should().Be("Carcass Jack");
                actual.Type.Should().Be("Varnished Coat");
                actual.Links.Min.Should().Be(6);
            }
        }
    }
}
