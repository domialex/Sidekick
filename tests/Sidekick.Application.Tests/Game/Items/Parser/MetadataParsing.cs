using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Xunit;

namespace Sidekick.Application.Tests.Game.Items.Parser
{
    [Collection(Collections.Mediator)]
    public class MetadataParsing
    {
        private readonly IMediator mediator;
        private readonly ItemTexts texts;

        public MetadataParsing(MediatorFixture fixture)
        {
            mediator = fixture.Mediator;
            texts = fixture.Texts;
        }

        [Fact]
        public async Task ChaosOrb()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ChaosOrb));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Chaos Orb", actual.Metadata.Type);
        }

        [Fact]
        public async Task ArcardeMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ArcardeMap));

            Assert.Equal(Class.Maps, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Arcade Map", actual.Metadata.Type);
        }

        [Fact]
        public async Task ScreamingEssenceOfWoe()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ScreamingEssenceOfWoe));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Screaming Essence of Woe", actual.Metadata.Type);
        }

        [Fact]
        public async Task SacrificeAtMidnight()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.SacrificeAtMidnight));

            Assert.Equal(Class.MapFragments, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Sacrifice at Midnight", actual.Metadata.Type);
        }

        [Fact]
        public async Task RitualSplinter()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.RitualSplinter));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Ritual Splinter", actual.Metadata.Type);
        }

        [Fact]
        public async Task TimelessEternalEmpireSplinter()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.TimelessEternalEmpireSplinter));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Timeless Eternal Empire Splinter", actual.Metadata.Type);
        }

        [Fact]
        public async Task DivineVessel()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.DivineVessel));

            Assert.Equal(Class.MapFragments, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Divine Vessel", actual.Metadata.Type);
        }

        [Fact]
        public async Task TributeToTheGoddess()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.TributeToTheGoddess));

            Assert.Equal(Class.MapFragments, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Tribute to the Goddess", actual.Metadata.Type);
        }

        [Fact]
        public async Task SplinterOfTul()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.SplinterOfTul));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Splinter of Tul", actual.Metadata.Type);
        }

        [Fact]
        public async Task RustedReliquaryScarab()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.RustedReliquaryScarab));

            Assert.Equal(Class.MapFragments, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Rusted Reliquary Scarab", actual.Metadata.Type);
        }

        [Fact]
        public async Task BoonOfJustice()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.BoonOfJustice));

            Assert.Equal(Class.DivinationCard, actual.Metadata.Class);
            Assert.Equal(Rarity.DivinationCard, actual.Metadata.Rarity);
            Assert.Equal(Category.DivinationCard, actual.Metadata.Category);
            Assert.Equal("Boon of Justice", actual.Metadata.Type);
        }

        [Fact]
        public async Task DaressosDefiance()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.DaressosDefiance));

            Assert.Equal(Class.BodyArmours, actual.Metadata.Class);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal(Category.Armour, actual.Metadata.Category);
            Assert.Equal("Daresso's Defiance", actual.Metadata.Name);
            Assert.Equal("Full Dragonscale", actual.Metadata.Type);
        }

        [Fact]
        public async Task ClearOil()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ClearOil));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Clear Oil", actual.Metadata.Type);
        }

        [Fact]
        public async Task BlightedSpiderLairMap()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.BlightedSpiderLairMap));

            Assert.Equal(Class.Maps, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Spider Lair Map", actual.Metadata.Type);
        }

        [Fact]
        public async Task SimulacrumSplinter()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.SimulacrumSplinter));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Simulacrum Splinter", actual.Metadata.Type);
        }

        [Fact]
        public async Task PerfectFossil()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.PerfectFossil));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Perfect Fossil", actual.Metadata.Type);
        }

        [Fact]
        public async Task PowerfulChaoticResonator()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.PowerfulChaoticResonator));

            Assert.Equal(Class.DelveStackableSocketableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Powerful Chaotic Resonator", actual.Metadata.Type);
        }

        [Fact]
        public async Task NoxiousCatalyst()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.NoxiousCatalyst));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Noxious Catalyst", actual.Metadata.Type);
        }

        [Fact]
        public async Task BloodProgenitorsBrain()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.BloodProgenitorsBrain));

            Assert.Equal(Class.MetamorphSample, actual.Metadata.Class);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal(Category.ItemisedMonster, actual.Metadata.Category);
            Assert.Equal("Blood Progenitor", actual.Metadata.Type);
        }

        [Fact]
        public async Task HallowedLifeFlask()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HallowedLifeFlask));

            Assert.Equal(Class.LifeFlasks, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Flask, actual.Metadata.Category);
            Assert.Equal("Hallowed Life Flask", actual.Metadata.Type);
        }

        [Fact]
        public async Task HallowedManaFlask()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HallowedManaFlask));

            Assert.Equal(Class.ManaFlasks, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Flask, actual.Metadata.Category);
            Assert.Equal("Hallowed Mana Flask", actual.Metadata.Type);
        }

        [Fact]
        public async Task ArcaneSurgeSupport()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ArcaneSurgeSupport));

            Assert.Equal(Class.SupportSkillGems, actual.Metadata.Class);
            Assert.Equal(Rarity.Gem, actual.Metadata.Rarity);
            Assert.Equal(Category.Gem, actual.Metadata.Category);
            Assert.Equal("Arcane Surge Support", actual.Metadata.Type);
        }

        [Fact]
        public async Task VoidSphere()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.VoidSphere));

            Assert.Equal(Class.ActiveSkillGems, actual.Metadata.Class);
            Assert.Equal(Rarity.Gem, actual.Metadata.Rarity);
            Assert.Equal(Category.Gem, actual.Metadata.Category);
            Assert.Equal("Void Sphere", actual.Metadata.Type);
        }

        [Fact]
        public async Task SacredHybridFlask()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.SacredHybridFlask));

            Assert.Equal(Class.HybridFlasks, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.Flask, actual.Metadata.Category);
            Assert.Equal("Sacred Hybrid Flask", actual.Metadata.Type);
        }

        [Fact]
        public async Task ViridianJewel()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ViridianJewel));

            Assert.Equal(Class.Jewel, actual.Metadata.Class);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal(Category.Jewel, actual.Metadata.Category);
            Assert.Equal("Viridian Jewel", actual.Metadata.Type);
        }

        [Fact]
        public async Task SmallClusterJewel()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.SmallClusterJewel));

            Assert.Equal(Class.Jewel, actual.Metadata.Class);
            Assert.Equal(Rarity.Magic, actual.Metadata.Rarity);
            Assert.Equal(Category.Jewel, actual.Metadata.Category);
            Assert.Equal("Small Cluster Jewel", actual.Metadata.Type);
        }

        [Fact]
        public async Task InscribedUltimatum()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.InscribedUltimatum));

            Assert.Equal(Class.MiscMapItems, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Map, actual.Metadata.Category);
            Assert.Equal("Inscribed Ultimatum", actual.Metadata.Type);
        }

        [Fact]
        public async Task Reefbane()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.Reefbane));

            Assert.Equal(Class.FishingRods, actual.Metadata.Class);
            Assert.Equal(Rarity.Unique, actual.Metadata.Rarity);
            Assert.Equal(Category.Weapon, actual.Metadata.Category);
            Assert.Equal("Reefbane", actual.Metadata.Name);
            Assert.Equal("Fishing Rod", actual.Metadata.Type);
        }

        [Fact]
        public async Task FarricChieftain()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.FarricChieftain));

            Assert.Equal(Class.StackableCurrency, actual.Metadata.Class);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal(Category.ItemisedMonster, actual.Metadata.Category);
            Assert.Equal("Farric Chieftain", actual.Metadata.Type);
        }

        [Fact]
        public async Task HeistTool()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HeistTool));

            Assert.Equal(Class.HeistTool, actual.Metadata.Class);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal(Category.HeistEquipment, actual.Metadata.Category);
            Assert.Equal("Lustrous Ward", actual.Metadata.Type);
        }

        [Fact]
        public async Task HeistCloak()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HeistCloak));

            Assert.Equal(Class.HeistCloak, actual.Metadata.Class);
            Assert.Equal(Rarity.Magic, actual.Metadata.Rarity);
            Assert.Equal(Category.HeistEquipment, actual.Metadata.Category);
            Assert.Equal("Torn Cloak", actual.Metadata.Type);
        }

        [Fact]
        public async Task HeistBrooch()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HeistBrooch));

            Assert.Equal(Class.HeistBrooch, actual.Metadata.Class);
            Assert.Equal(Rarity.Normal, actual.Metadata.Rarity);
            Assert.Equal(Category.HeistEquipment, actual.Metadata.Category);
            Assert.Equal("Silver Brooch", actual.Metadata.Type);
        }

        [Fact]
        public async Task HeistGear()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HeistGear));

            Assert.Equal(Class.HeistGear, actual.Metadata.Class);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal(Category.HeistEquipment, actual.Metadata.Category);
            Assert.Equal("Rough Sharpening Stone", actual.Metadata.Type);
        }

        [Fact]
        public async Task HeistTarget()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.HeistTarget));

            Assert.Equal(Class.HeistTarget, actual.Metadata.Class);
            Assert.Equal(Rarity.Currency, actual.Metadata.Rarity);
            Assert.Equal(Category.Currency, actual.Metadata.Category);
            Assert.Equal("Alchemical Chalice", actual.Metadata.Type);
        }

        [Fact]
        public async Task ThiefTrinket()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.ThiefTrinket));

            Assert.Equal(Class.Trinkets, actual.Metadata.Class);
            Assert.Equal(Rarity.Rare, actual.Metadata.Rarity);
            Assert.Equal(Category.Accessory, actual.Metadata.Category);
            Assert.Equal("Thief's Trinket", actual.Metadata.Type);
        }

        [Fact]
        public async Task AbyssJewel()
        {
            var actual = await mediator.Send(new ParseItemCommand(texts.AbyssJewel));

            Assert.Equal(Class.AbyssJewel, actual.Metadata.Class);
            Assert.Equal(Rarity.Magic, actual.Metadata.Rarity);
            Assert.Equal(Category.Jewel, actual.Metadata.Category);
            Assert.Equal("Hypnotic Eye Jewel", actual.Metadata.Type);
        }
    }
}
