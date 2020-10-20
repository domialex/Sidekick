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
        private readonly SidekickSettings settings;

        private const string EnglishLanguageName = "English";

        public LanguageProvider(ILogger<LanguageProvider> logger,
            SidekickSettings settings)
        {
            this.logger = logger;
            this.settings = settings;

            AvailableLanguages = new List<LanguageAttribute>();
            foreach (var type in typeof(LanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<LanguageAttribute>();
                attribute.ImplementationType = type;
                AvailableLanguages.Add(attribute);
            }

            if (!SetLanguage(settings.Language_Parser))
            {
                SetLanguage(EnglishLanguageName);
            }
        }

        private List<LanguageAttribute> AvailableLanguages { get; set; }

        public ILanguage EnglishLanguage => CreateLanguageInstance(AvailableLanguages.First(x => x.Name == EnglishLanguageName).ImplementationType);

        public bool IsEnglish => Current.Name == EnglishLanguageName;

        public LanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public ILanguage Language { get; private set; }

        private bool SetLanguage(string name)
        {
            var language = AvailableLanguages.Find(x => x.Name == name);

            if (language == null)
            {
                logger.LogInformation("Couldn't find language matching {language}.", name);
                return false;
            }

            if (Language == null || Language.DescriptionRarity != language.DescriptionRarity)
            {
                logger.LogInformation("Changed active language support to {language}.", language.Name);
                Language = CreateLanguageInstance(language.ImplementationType);

                settings.Language_Parser = name;
                settings.Save();

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
