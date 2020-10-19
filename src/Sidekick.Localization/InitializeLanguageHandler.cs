using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Localization
{
    public class InitializeLanguageHandler : INotificationHandler<InitializeLanguageNotification>
    {
        private readonly IUILanguageProvider uILanguageProvider;
        private readonly SidekickSettings settings;

        public InitializeLanguageHandler(
            IUILanguageProvider uILanguageProvider,
            SidekickSettings settings)
        {
            this.uILanguageProvider = uILanguageProvider;
            this.settings = settings;
        }

        public Task Handle(InitializeLanguageNotification notification, CancellationToken cancellationToken)
        {
            notification.OnStart("UI Language");

            var current = uILanguageProvider.AvailableLanguages.FirstOrDefault(x => x.Name == settings.Language_UI);
            if (current != null)
            {
                uILanguageProvider.SetLanguage(settings.Language_UI);
            }
            else
            {
                uILanguageProvider.SetLanguage(uILanguageProvider.AvailableLanguages.FirstOrDefault().Name);
            }

            notification.OnEnd("UI Language");

            return Task.CompletedTask;
        }
    }
}
