using System.Collections.Generic;

namespace Sidekick.Domain.Game.Languages
{
    public interface ILanguageProvider
    {
        List<LanguageAttribute> AvailableLanguages { get; set; }

        ILanguage EnglishLanguage { get; set; }

        bool IsEnglish { get; }

        ILanguage Language { get; set; }

        LanguageAttribute Current { get; }
    }
}
