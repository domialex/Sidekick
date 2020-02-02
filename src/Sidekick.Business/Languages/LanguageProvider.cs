using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Languages.Implementations;
using Sidekick.Core.Extensions;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;

namespace Sidekick.Business.Languages
{
    public class LanguageProvider : ILanguageProvider
    {
        private readonly ILogger logger;
        private readonly IInitializer initializeService;

        public LanguageProvider(ILogger logger,
            IInitializer initializeService)
        {
            this.logger = logger;
            this.initializeService = initializeService;

            AvailableLanguages = new List<LanguageAttribute>();
            foreach (var type in typeof(LanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<LanguageAttribute>();
                attribute.ImplementationType = type;
                AvailableLanguages.Add(attribute);
            }

            Language = new LanguageEN();
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
            foreach (var language in AvailableLanguages)
            {
                if (itemDescription.Contains(language.DescriptionRarity))
                {
                    if (Language.DescriptionRarity != language.DescriptionRarity)
                    {
                        logger.Log($"Changed language support to {language.Name}.");
                        Language = (ILanguage)Activator.CreateInstance(language.ImplementationType);

                        await initializeService.Reset();
                        await initializeService.Initialize();
                    }

                    return;
                }
            }

            logger.Log("This Path of Exile language is not yet supported.");
        }
    }
}
