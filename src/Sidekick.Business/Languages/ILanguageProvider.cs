using System.Collections.Generic;

namespace Sidekick.Business.Languages
{
    public interface ILanguageProvider
    {
        List<LanguageAttribute> AvailableLanguages { get; }

        ILanguage EnglishLanguage { get; }

        bool IsEnglish { get; }

        ILanguage Language { get; }

        LanguageAttribute Current { get; }

        bool SetLanguage(string languageCode);
    }
}
