using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Game.Languages;

namespace Sidekick.Application.Game.Languages
{
    public class GameLanguageProvider : IGameLanguageProvider
    {
        private const string EnglishLanguageCode = "en";

        public List<GameLanguageAttribute> AvailableLanguages { get; set; }

        public IGameLanguage EnglishLanguage { get; set; }

        public bool IsEnglish => Current.LanguageCode == EnglishLanguageCode;

        public GameLanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public IGameLanguage Language { get; set; }
    }
}
