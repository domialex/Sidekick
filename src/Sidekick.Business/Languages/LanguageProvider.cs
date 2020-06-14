using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Caches;
using Sidekick.Core.Extensions;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Languages
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly ILogger logger;
        private readonly IInitializer initializeService;
        private readonly ICacheService cacheService;
        private readonly SidekickSettings settings;

        private readonly string defaultLanguageName = "English";

        public LanguageProvider(ILogger logger,
            IInitializer initializeService,
            ICacheService cacheService,
            SidekickSettings settings)
        {
            this.logger = logger.ForContext(GetType());
            this.initializeService = initializeService;
            this.cacheService = cacheService;
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
                SetLanguage(defaultLanguageName);
            }
        }

        private List<LanguageAttribute> AvailableLanguages { get; set; }

        public ILanguage DefaultLanguage => CreateLanguageInstance(AvailableLanguages.First(x => x.Name == defaultLanguageName).ImplementationType);

        public bool IsEnglish => Current.Name == defaultLanguageName;

        public LanguageAttribute Current => AvailableLanguages.First(x => x.DescriptionRarity == Language.DescriptionRarity);

        public ILanguage Language { get; private set; }

        /// <summary>
        /// Every item should start with Rarity in the first line.
        /// This will force the TradeClient to refetch the Public API's data if needed.
        /// </summary>
        public async Task FindAndSetLanguage(string itemDescription)
        {
            var language = AvailableLanguages.Find(x => itemDescription.StartsWith(x.DescriptionRarity));

            if (language != null && SetLanguage(language.Name))
            {
                await cacheService.Clear();
                await initializeService.Initialize();
            }
        }

        private bool SetLanguage(string name)
        {
            var language = AvailableLanguages.Find(x => x.Name == name);

            if (language == null)
            {
                logger.Information("Couldn't find language matching {language}.", name);
                return false;
            }

            if (Language == null || Language.DescriptionRarity != language.DescriptionRarity)
            {
                logger.Information("Changed active language support to {language}.", language.Name);
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
