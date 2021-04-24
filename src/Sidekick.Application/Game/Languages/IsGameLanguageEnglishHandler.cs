using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sidekick.Domain.Game.Languages.Commands
{
    public class IsGameLanguageEnglishHandler : IQueryHandler<IsGameLanguageEnglishQuery, bool>
    {
        private const string EnglishLanguageCode = "en";

        private readonly IGameLanguageProvider gameLanguageProvider;

        public IsGameLanguageEnglishHandler(
            IGameLanguageProvider gameLanguageProvider)
        {
            this.gameLanguageProvider = gameLanguageProvider;
        }

        public Task<bool> Handle(IsGameLanguageEnglishQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(gameLanguageProvider.Language.LanguageCode == EnglishLanguageCode);
        }
    }
}
