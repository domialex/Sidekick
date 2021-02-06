using Sidekick.Domain.Game.Languages;

namespace Sidekick.Application.Game.Languages
{
    public class GameLanguageProvider : IGameLanguageProvider
    {
        public IGameLanguage Language { get; set; }
    }
}
