using System.Collections.Generic;

namespace Sidekick.Domain.Game.Languages
{
    public interface IGameLanguageProvider
    {
        List<GameLanguageAttribute> AvailableLanguages { get; set; }

        IGameLanguage EnglishLanguage { get; set; }

        bool IsEnglish { get; }

        IGameLanguage Language { get; set; }

        GameLanguageAttribute Current { get; }
    }
}
