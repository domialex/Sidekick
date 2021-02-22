using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Delve;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetDelveCheatsheetHandler : IQueryHandler<GetDelveCheatsheetQuery, DelveLeague>
    {
        private readonly DelveResources resources;

        public GetDelveCheatsheetHandler(
            DelveResources resources)
        {
            this.resources = resources;
        }

        public Task<DelveLeague> Handle(GetDelveCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var regions = new List<DelveRegion>
            {
                new DelveRegion(resources.Mines)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.SerratedFossil, RewardValue.Medium),
                        new DelveFossil(resources.MetallicFossil, RewardValue.Low),
                        new DelveFossil(resources.AethericFossil, RewardValue.NoValue),
                        new DelveFossil(resources.PristineFossil, RewardValue.NoValue),
                    }
                },

                new DelveRegion(resources.MagmaFissure)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.EnchantedFossil, RewardValue.Medium),
                        new DelveFossil(resources.PrismaticFossil, RewardValue.Low),
                        new DelveFossil(resources.EncrustedFossil, RewardValue.NoValue, true),
                        new DelveFossil(resources.PristineFossil, RewardValue.NoValue),
                        new DelveFossil(resources.ScorchedFossil, RewardValue.NoValue),
                    }
                },

                new DelveRegion(resources.SulfurVents)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.PerfectFossil, RewardValue.Medium),
                        new DelveFossil(resources.MetallicFossil, RewardValue.Low),
                        new DelveFossil(resources.AethericFossil, RewardValue.NoValue),
                        new DelveFossil(resources.EncrustedFossil, RewardValue.NoValue, true),
                    }
                },

                new DelveRegion(resources.FrozenHollow)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.SanctifiedFossil, RewardValue.High, true),
                        new DelveFossil(resources.SerratedFossil, RewardValue.Medium),
                        new DelveFossil(resources.ShudderingFossil, RewardValue.Medium),
                        new DelveFossil(resources.PrismaticFossil, RewardValue.Low),
                        new DelveFossil(resources.FrigidFossil, RewardValue.NoValue),
                    }
                },

                new DelveRegion(resources.FungalCaverns)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.CorrodedFossil, RewardValue.Medium),
                        new DelveFossil(resources.GildedFossil, RewardValue.Medium, true),
                        new DelveFossil(resources.PerfectFossil, RewardValue.Medium),
                        new DelveFossil(resources.AberrantFossil, RewardValue.Low),
                        new DelveFossil(resources.DenseFossil, RewardValue.Low),
                    }
                },

                new DelveRegion(resources.PetrifiedForest)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.SanctifiedFossil, RewardValue.High, true),
                        new DelveFossil(resources.CorrodedFossil, RewardValue.Medium),
                        new DelveFossil(resources.BoundFossil, RewardValue.Low),
                        new DelveFossil(resources.DenseFossil, RewardValue.Low),
                        new DelveFossil(resources.JaggedFossil, RewardValue.NoValue),
                    }
                },

                new DelveRegion(resources.AbyssalDepths)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.GildedFossil, RewardValue.Medium, true),
                        new DelveFossil(resources.AberrantFossil, RewardValue.Low),
                        new DelveFossil(resources.BoundFossil, RewardValue.Low),
                        new DelveFossil(resources.LucentFossil, RewardValue.NoValue, true),
                    }
                },

                new DelveRegion(resources.FossilRoom)
                {
                    Fossils = new List<DelveFossil>()
                    {
                        new DelveFossil(resources.BloodstainedFossil, RewardValue.High),
                        new DelveFossil(resources.FacetedFossil, RewardValue.High),
                        new DelveFossil(resources.FracturedFossil, RewardValue.High),
                        new DelveFossil(resources.GlyphicFossil, RewardValue.High),
                        new DelveFossil(resources.HollowFossil, RewardValue.High),
                        new DelveFossil(resources.TangledFossil, RewardValue.Medium),
                    }
                },
            };

            return Task.FromResult(new DelveLeague()
            {
                Regions = regions
            });
        }
    }
}
