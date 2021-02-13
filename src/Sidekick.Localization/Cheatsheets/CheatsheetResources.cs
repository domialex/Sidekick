using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Cheatsheets
{
    public class CheatsheetResources
    {
        private readonly IStringLocalizer<CheatsheetResources> localizer;

        public CheatsheetResources(IStringLocalizer<CheatsheetResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Cheatsheets => localizer["Cheatsheets"];
        public string Betrayal => localizer["Betrayal"];
        public string Delve => localizer["Delve"];
        public string Blight => localizer["Blight"];
        public string Heist => localizer["Heist"];
        public string Incursion => localizer["Incursion"];
        public string Metamorph => localizer["Metamorph"];
        public string Leagues => localizer["Leagues"];
    }
}
