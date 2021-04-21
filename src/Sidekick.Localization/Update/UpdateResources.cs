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

        public string Available => localizer["Available"];
        public string AvailableText(string version) => localizer["AvailableText", version];
        public string Downloaded => localizer["Downloaded"];
        public string DownloadedText(string version) => localizer["DownloadedText", version];
    }
}
