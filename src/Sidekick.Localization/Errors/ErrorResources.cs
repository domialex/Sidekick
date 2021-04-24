using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Errors
{
    public class ErrorResources
    {
        private readonly IStringLocalizer<ErrorResources> localizer;

        public ErrorResources(IStringLocalizer<ErrorResources> localizer)
        {
            this.localizer = localizer;
        }

        public string AvailableInEnglishError => localizer["AvailableInEnglishError"];
        public string InvalidItemError => localizer["InvalidItemError"];
        public string ParserError => localizer["ParserError"];
        public string Title => localizer["Title"];
        public string Close => localizer["Close"];
        public string Error => localizer["Error"];
    }
}
