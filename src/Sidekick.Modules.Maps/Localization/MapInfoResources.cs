using Microsoft.Extensions.Localization;

namespace Sidekick.Modules.Maps.Localization
{
    public class MapInfoResources
    {
        private readonly IStringLocalizer<MapInfoResources> localizer;

        public MapInfoResources(IStringLocalizer<MapInfoResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Title => localizer["Title"];
        public string Is_Safe => localizer["Is_Safe"];
        public string Is_Unsafe => localizer["Is_Unsafe"];
    }
}
