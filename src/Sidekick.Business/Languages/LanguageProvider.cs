using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Core.Extensions;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Languages
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly ILogger logger;
        private readonly IInitializer initializeService;
        private readonly SidekickSettings settings;

        public LanguageProvider(ILogger logger,
            IInitializer initializeService,
            SidekickSettings settings)
        {
            this.logger = logger;
            this.initializeService = initializeService;
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
                SetLanguage(DefaultLanguage);
            }
        }

        private List<LanguageAttribute> AvailableLanguages { get; set; }

        public string DefaultLanguage => "English";

        public bool IsEnglish => Current.Name == "English";

        public LanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public ILanguage Language { get; private set; }

        /// <summary>
        /// Every item should start with Rarity in the first line. 
        /// This will force the TradeClient to refetch the Public API's data if needed.
        /// </summary>
        public async Task FindAndSetLanguage(string itemDescription)
        {
            var language = AvailableLanguages.FirstOrDefault(x => itemDescription.Contains(x.DescriptionRarity));

            if (language != null && SetLanguage(language.Name))
            {
                await initializeService.Initialize();
            }
        }

        private bool SetLanguage(string name)
        {
            var language = AvailableLanguages.FirstOrDefault(x => x.Name == name);

            if (language == null)
            {
                logger.Log($"Couldn't find language {language.Name}.");
                return false;
            }

            if (Language == null || Language.DescriptionRarity != language.DescriptionRarity)
            {
                logger.Log($"Changed language support to {language.Name}.");
                Language = (ILanguage)Activator.CreateInstance(language.ImplementationType);

                settings.Language_Parser = name;
                settings.Save();

                return true;
            }

            return false;
        }
    }
}
