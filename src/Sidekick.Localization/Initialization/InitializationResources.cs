using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Initialization
{
    public class InitializationResources
    {
        private readonly IStringLocalizer<InitializationResources> localizer;

        public InitializationResources(IStringLocalizer<InitializationResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Error => localizer["Error"];
        public string NewLeagues => localizer["NewLeagues"];
        public string Notification_Message => localizer["Notification_Message"];
        public string Notification_Title => localizer["Notification_Title"];
        public string Ready => localizer["Ready"];
        public string Title(int completed, int count) => localizer["Title", completed, count];
        public string UpdateAvailable => localizer["UpdateAvailable"];
        public string UpdateTitle => localizer["UpdateTitle"];
    }
}
