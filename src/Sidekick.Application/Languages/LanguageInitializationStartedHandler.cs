using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Settings;
using Sidekick.Extensions;

namespace Sidekick.Application.Languages
{
    public class LanguageInitializationStartedHandler : INotificationHandler<LanguageInitializationStarted>
    {
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ISidekickSettings settings;
        private readonly IMediator mediator;

        public LanguageInitializationStartedHandler(
            IGameLanguageProvider gameLanguageProvider,
            ISidekickSettings settings,
            IMediator mediator)
        {
            this.gameLanguageProvider = gameLanguageProvider;
            this.settings = settings;
            this.mediator = mediator;
        }

        public async Task Handle(LanguageInitializationStarted notification, CancellationToken cancellationToken)
        {
            gameLanguageProvider.AvailableLanguages = new List<GameLanguageAttribute>();
            foreach (var type in typeof(GameLanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<GameLanguageAttribute>();
                attribute.ImplementationType = type;
                gameLanguageProvider.AvailableLanguages.Add(attribute);
            }

            await mediator.Send(new SetGameLanguageCommand(settings.Language_Parser));

            gameLanguageProvider.EnglishLanguage = (IGameLanguage)Activator.CreateInstance(gameLanguageProvider.AvailableLanguages.First(x => x.LanguageCode == "en").ImplementationType);
        }
    }
}
