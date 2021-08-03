using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;

namespace Sidekick.Application.Game.Languages
{
    public class SetGameLanguageHandler : ICommandHandler<SetGameLanguageCommand>
    {
        private readonly IMediator mediator;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ILogger<SetGameLanguageHandler> logger;

        public SetGameLanguageHandler(
            IMediator mediator,
            IGameLanguageProvider gameLanguageProvider,
            ILogger<SetGameLanguageHandler> logger)
        {
            this.mediator = mediator;
            this.gameLanguageProvider = gameLanguageProvider;
            this.logger = logger;
        }

        public async Task<Unit> Handle(SetGameLanguageCommand request, CancellationToken cancellationToken)
        {
            var availableLanguages = await mediator.Send(new GetGameLanguagesQuery());
            var language = availableLanguages.Find(x => x.LanguageCode == request.LanguageCode);

            if (language == null)
            {
                logger.LogWarning("Couldn't find language matching {language}.", request.LanguageCode);
                return Unit.Value;
            }

            gameLanguageProvider.Language = (IGameLanguage)Activator.CreateInstance(language.ImplementationType);

            return Unit.Value;
        }
    }
}
