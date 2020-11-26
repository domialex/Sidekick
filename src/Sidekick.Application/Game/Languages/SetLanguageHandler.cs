using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;

namespace Sidekick.Application.Game.Languages
{
    public class SetLanguageHandler : ICommandHandler<SetGameLanguageCommand>
    {
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ILogger<SetLanguageHandler> logger;

        public SetLanguageHandler(
            IGameLanguageProvider gameLanguageProvider,
            ILogger<SetLanguageHandler> logger)
        {
            this.gameLanguageProvider = gameLanguageProvider;
            this.logger = logger;
        }

        public Task<Unit> Handle(SetGameLanguageCommand request, CancellationToken cancellationToken)
        {
            var language = gameLanguageProvider.AvailableLanguages.Find(x => x.LanguageCode == request.LanguageCode);

            if (language == null)
            {
                logger.LogWarning("Couldn't find language matching {language}.", request.LanguageCode);
                return Unit.Task;
            }

            gameLanguageProvider.Language = (IGameLanguage)Activator.CreateInstance(language.ImplementationType);

            return Unit.Task;
        }
    }
}
