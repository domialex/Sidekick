using Microsoft.Extensions.Localization;

namespace Sidekick.Presentation.Blazor.Localization
{
    public class TrayResources
    {
        private readonly IStringLocalizer<TrayResources> localizer;

        public TrayResources(IStringLocalizer<TrayResources> localizer)
        {
            this.localizer = localizer;
        }

        public string About => localizer["About"];
        public string Cheatsheets => localizer["Cheatsheets"];
        public string Exit => localizer["Exit"];
        public string Settings => localizer["Settings"];
        public string Title => localizer["Title"];

    }
}
