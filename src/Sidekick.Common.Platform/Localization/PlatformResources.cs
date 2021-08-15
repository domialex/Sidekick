using Microsoft.Extensions.Localization;

namespace Sidekick.Common.Platforms.Localization
{
    public class PlatformResources
    {
        private readonly IStringLocalizer<PlatformResources> localizer;

        public PlatformResources(IStringLocalizer<PlatformResources> localizer)
        {
            this.localizer = localizer;
        }

        public string AdminError => localizer["AdminError"];
        public string AlreadyRunningText => localizer["AlreadyRunningText"];
        public string RestartText => localizer["RestartText"];
    }
}
