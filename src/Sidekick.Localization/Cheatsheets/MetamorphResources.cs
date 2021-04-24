using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Cheatsheets
{
    public class MetamorphResources
    {
        private readonly IStringLocalizer<MetamorphResources> localizer;

        public MetamorphResources(IStringLocalizer<MetamorphResources> localizer)
        {
            this.localizer = localizer;
        }

        public string AbrasiveCatalyst => localizer["AbrasiveCatalyst"];
        public string AbrasiveCatalystEffect => localizer["AbrasiveCatalystEffect"];
        public string FertileCatalyst => localizer["FertileCatalyst"];
        public string FertileCatalystEffect => localizer["FertileCatalystEffect"];
        public string ImbuedCatalyst => localizer["ImbuedCatalyst"];
        public string ImbuedCatalystEffect => localizer["ImbuedCatalystEffect"];
        public string IntrinsicCatalyst => localizer["IntrinsicCatalyst"];
        public string IntrinsicCatalystEffect => localizer["IntrinsicCatalystEffect"];
        public string PrismaticCatalyst => localizer["PrismaticCatalyst"];
        public string PrismaticCatalystEffect => localizer["PrismaticCatalystEffect"];
        public string TemperingCatalyst => localizer["TemperingCatalyst"];
        public string TemperingCatalystEffect => localizer["TemperingCatalystEffect"];
        public string TurbulentCatalyst => localizer["TurbulentCatalyst"];
        public string TurbulentCatalystEffect => localizer["TurbulentCatalystEffect"];
    }
}
