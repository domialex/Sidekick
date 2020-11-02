using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Languages
{
    public class LanguageProvider : ILanguageProvider
    {
        private const string EnglishLanguageCode = "en";

        public List<LanguageAttribute> AvailableLanguages { get; set; }

        public ILanguage EnglishLanguage { get; set; }

        public bool IsEnglish => Current.LanguageCode == EnglishLanguageCode;

        public LanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public ILanguage Language { get; set; }
    }
}
