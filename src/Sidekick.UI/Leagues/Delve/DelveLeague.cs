using System.Collections.Generic;
using Sidekick.Localization.Leagues.Delve;

namespace Sidekick.UI.Leagues.Delve
{
    public class DelveLeague
    {
        public DelveLeague()
        {
            Regions = new List<DelveRegion>();

            Regions.Add(new DelveRegion(DelveResources.Mines)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.SerratedFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.MetallicFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.AethericFossil, RewardValue.NoValue),
                    new DelveFossil(DelveResources.PristineFossil, RewardValue.NoValue),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.MagmaFissure)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.EnchantedFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.PrismaticFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.EncrustedFossil, RewardValue.NoValue, true),
                    new DelveFossil(DelveResources.PristineFossil, RewardValue.NoValue),
                    new DelveFossil(DelveResources.ScorchedFossil, RewardValue.NoValue),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.SulfurVents)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.PerfectFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.MetallicFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.AethericFossil, RewardValue.NoValue),
                    new DelveFossil(DelveResources.EncrustedFossil, RewardValue.NoValue, true),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.FrozenHollow)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.SanctifiedFossil, RewardValue.High, true),
                    new DelveFossil(DelveResources.SerratedFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.ShudderingFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.PrismaticFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.FrigidFossil, RewardValue.NoValue),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.FungalCaverns)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.CorrodedFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.GildedFossil, RewardValue.Medium, true),
                    new DelveFossil(DelveResources.PerfectFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.AberrantFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.DenseFossil, RewardValue.Low),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.PetrifiedForest)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.SanctifiedFossil, RewardValue.High, true),
                    new DelveFossil(DelveResources.CorrodedFossil, RewardValue.Medium),
                    new DelveFossil(DelveResources.BoundFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.DenseFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.JaggedFossil, RewardValue.NoValue),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.AbyssalDepths)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.GildedFossil, RewardValue.Medium, true),
                    new DelveFossil(DelveResources.AberrantFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.BoundFossil, RewardValue.Low),
                    new DelveFossil(DelveResources.LucentFossil, RewardValue.NoValue, true),
                }
            });

            Regions.Add(new DelveRegion(DelveResources.FossilRoom)
            {
                Fossils = new List<DelveFossil>()
                {
                    new DelveFossil(DelveResources.BloodstainedFossil, RewardValue.High),
                    new DelveFossil(DelveResources.FacetedFossil, RewardValue.High),
                    new DelveFossil(DelveResources.FracturedFossil, RewardValue.High),
                    new DelveFossil(DelveResources.GlyphicFossil, RewardValue.High),
                    new DelveFossil(DelveResources.HollowFossil, RewardValue.High),
                    new DelveFossil(DelveResources.TangledFossil, RewardValue.Medium),
                }
            });
        }

        public List<DelveRegion> Regions { get; set; }
    }
}
