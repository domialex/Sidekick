using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Setup
{
    public class SetupResources
    {
        private readonly IStringLocalizer<SetupResources> resources;

        public SetupResources(IStringLocalizer<SetupResources> resources)
        {
            this.resources = resources;
        }

        public string Exit => resources["Exit"];
        public string Title => resources["Title"];
    }
}
