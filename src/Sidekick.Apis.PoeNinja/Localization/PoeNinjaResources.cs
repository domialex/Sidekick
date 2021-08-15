using Microsoft.Extensions.Localization;

namespace Sidekick.Apis.PoeNinja.Localization
{
    public class PoeNinjaResources
    {
        private readonly IStringLocalizer<PoeNinjaResources> localizer;

        public PoeNinjaResources(IStringLocalizer<PoeNinjaResources> localizer)
        {
            this.localizer = localizer;
        }

        public string PoeNinja => localizer["PoeNinja"];
        public string LastUpdated => localizer["LastUpdated"];

    }
}
