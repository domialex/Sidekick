using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Languages;
using Sidekick.Domain.Languages.Commands;
using Sidekick.Extensions;

namespace Sidekick.Application.Languages
{
    public class LanguageInitializationStartedHandler : INotificationHandler<LanguageInitializationStarted>
    {
        private readonly ILanguageProvider languageProvider;
        private readonly SidekickSettings settings;
        private readonly IMediator mediator;

        public LanguageInitializationStartedHandler(
            ILanguageProvider languageProvider,
            SidekickSettings settings,
            IMediator mediator)
        {
            this.languageProvider = languageProvider;
            this.settings = settings;
            this.mediator = mediator;
        }

        public async Task Handle(LanguageInitializationStarted notification, CancellationToken cancellationToken)
        {
            languageProvider.AvailableLanguages = new List<LanguageAttribute>();
            foreach (var type in typeof(LanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<LanguageAttribute>();
                attribute.ImplementationType = type;
                languageProvider.AvailableLanguages.Add(attribute);
            }

            await mediator.Send(new SetLanguageCommand(settings.Language_Parser));

            languageProvider.EnglishLanguage = (ILanguage)Activator.CreateInstance(languageProvider.AvailableLanguages.First(x => x.LanguageCode == "en").ImplementationType);
        }
    }
}
