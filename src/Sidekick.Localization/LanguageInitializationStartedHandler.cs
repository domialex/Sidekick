using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Settings;

namespace Sidekick.Localization
{
    public class LanguageInitializationStartedHandler : INotificationHandler<LanguageInitializationStarted>
    {
        private readonly IUILanguageProvider uILanguageProvider;
        private readonly ISidekickSettings settings;

        public LanguageInitializationStartedHandler(
            IUILanguageProvider uILanguageProvider,
            ISidekickSettings settings)
        {
            this.uILanguageProvider = uILanguageProvider;
            this.settings = settings;
        }

        public Task Handle(LanguageInitializationStarted notification, CancellationToken cancellationToken)
        {
            var current = uILanguageProvider.AvailableLanguages.FirstOrDefault(x => x.Name == settings.Language_UI);
            if (current != null)
            {
                uILanguageProvider.SetLanguage(settings.Language_UI);
            }
            else
            {
                uILanguageProvider.SetLanguage(uILanguageProvider.AvailableLanguages.FirstOrDefault().Name);
            }

            return Task.CompletedTask;
        }
    }
}
