using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Extensions;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Languages
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly ILogger logger;

        private const string EnglishLanguageCode = "en";

        public LanguageProvider(
            ILogger<LanguageProvider> logger,
            SidekickSettings settings)
        {
            this.logger = logger;

            AvailableLanguages = new List<LanguageAttribute>();
            foreach (var type in typeof(LanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<LanguageAttribute>();
                attribute.ImplementationType = type;
                AvailableLanguages.Add(attribute);
            }

            if (!SetLanguage(settings.Language_Parser))
            {
                SetLanguage(EnglishLanguageCode);
            }

            EnglishLanguage = CreateLanguageInstance(AvailableLanguages.First(x => x.LanguageCode == EnglishLanguageCode).ImplementationType);
        }

        public List<LanguageAttribute> AvailableLanguages { get; private set; }

        public ILanguage EnglishLanguage { get; private set; }

        public bool IsEnglish => Current.LanguageCode == EnglishLanguageCode;

        public LanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public ILanguage Language { get; private set; }

        public bool SetLanguage(string languageCode)
        {
            var language = AvailableLanguages.Find(x => x.LanguageCode == languageCode);

            if (language == null)
            {
                logger.LogInformation("Couldn't find language matching {language}.", languageCode);
                return false;
            }

            if (Language?.DescriptionRarity != language.DescriptionRarity)
            {
                logger.LogInformation("Changed active language support to {language}.", language.Name);
                Language = CreateLanguageInstance(language.ImplementationType);

                return true;
            }

            return false;
        }

        private ILanguage CreateLanguageInstance(Type type)
        {
            return (ILanguage)Activator.CreateInstance(type);
        }
    }
}
