using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Update
{
    public class UpdateResources
    {
        private readonly IStringLocalizer<UpdateResources> localizer;

        public UpdateResources(IStringLocalizer<UpdateResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Downloaded => localizer["Downloaded"];
        public string Downloading(string version) => localizer["Downloading", version];
        public string Failed => localizer["Failed"];
    }
}
