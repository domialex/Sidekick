using Microsoft.Extensions.Localization;

namespace Sidekick.Modules.Cheatsheets.Localization
{
    public class DelveResources
    {
        private readonly IStringLocalizer<DelveResources> localizer;

        public DelveResources(IStringLocalizer<DelveResources> localizer)
        {
            this.localizer = localizer;
        }

        public string AberrantFossil => localizer["AberrantFossil"];
        public string AbyssalDepths => localizer["AbyssalDepths"];
        public string AethericFossil => localizer["AethericFossil"];
        public string BloodstainedFossil => localizer["BloodstainedFossil"];
        public string BoundFossil => localizer["BoundFossil"];
        public string CorrodedFossil => localizer["CorrodedFossil"];
        public string DenseFossil => localizer["DenseFossil"];
        public string EnchantedFossil => localizer["EnchantedFossil"];
        public string EncrustedFossil => localizer["EncrustedFossil"];
        public string FacetedFossil => localizer["FacetedFossil"];
        public string FossilRoom => localizer["FossilRoom"];
        public string FracturedFossil => localizer["FracturedFossil"];
        public string FrigidFossil => localizer["FrigidFossil"];
        public string FrozenHollow => localizer["FrozenHollow"];
        public string FungalCaverns => localizer["FungalCaverns"];
        public string GildedFossil => localizer["GildedFossil"];
        public string GlyphicFossil => localizer["GlyphicFossil"];
        public string HollowFossil => localizer["HollowFossil"];
        public string Information => localizer["Information"];
        public string JaggedFossil => localizer["JaggedFossil"];
        public string FracturedWall => localizer["FracturedWall"];
        public string LucentFossil => localizer["LucentFossil"];
        public string MagmaFissure => localizer["MagmaFissure"];
        public string MetallicFossil => localizer["MetallicFossil"];
        public string Mines => localizer["Mines"];
        public string PerfectFossil => localizer["PerfectFossil"];
        public string PetrifiedForest => localizer["PetrifiedForest"];
        public string PrismaticFossil => localizer["PrismaticFossil"];
        public string PristineFossil => localizer["PristineFossil"];
        public string SanctifiedFossil => localizer["SanctifiedFossil"];
        public string ScorchedFossil => localizer["ScorchedFossil"];
        public string SerratedFossil => localizer["SerratedFossil"];
        public string ShudderingFossil => localizer["ShudderingFossil"];
        public string SulfurVents => localizer["SulfurVents"];
        public string TangledFossil => localizer["TangledFossil"];
    }
}
