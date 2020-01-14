using Sidekick.Business.Localization.Locales;
using Sidekick.Business.Loggers;
using Sidekick.Core.DependencyInjection.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Localization
{
    [SidekickService(typeof(ILanguageService))]
    public class LanguageService : ILanguageService
    {
        private readonly ILogger logger;

        public LanguageService(ILogger logger)
        {
            this.logger = logger;

            Language = new LanguageEN();
            Current = LanguageEnum.English;
        }

        public ILanguage Language { get; private set; }
        public LanguageEnum Current { get; private set; }

        private Dictionary<string, LanguageEnum> RarityToLanguageDictionary = new Dictionary<string, LanguageEnum>()
        {
            { "Rarity: ", LanguageEnum.English },
            { "Rareté: ", LanguageEnum.French },
            { "Seltenheit: ", LanguageEnum.German },
            { "아이템 희귀도: ", LanguageEnum.Korean },
            { "Raridade: ", LanguageEnum.Portuguese },
            { "Редкость: ", LanguageEnum.Russian },
            { "Rareza: ", LanguageEnum.Spanish },
            { "ความหายาก: ", LanguageEnum.Thai }
        };

        /// <summary>
        /// Every item should start with Rarity in the first line. 
        /// This will force the TradeClient to refetch the Public API's data if needed.
        /// </summary>
        public async Task<bool> FindAndSetLanguageProvider(string itemDescription)
        {
            foreach (var item in RarityToLanguageDictionary)
            {
                if (itemDescription.Contains(item.Key))
                {
                    if (Current != item.Value)
                    {
                        logger.Log($"Changed language support to {item.Value}.");
                        Current = item.Value;
                        Language = GetLanguageProvider(item.Value);
                        TradeClient.Dispose();
                        return await TradeClient.Initialize();
                    }

                    return true;
                }
            }

            logger.Log("This Path of Exile language is not yet supported.");
            return false;
        }

        public static ILanguage GetLanguageProvider(LanguageEnum language)
        {
            switch (language)
            {
                case LanguageEnum.French:
                    return new LanguageFR();
                case LanguageEnum.German:
                    return new LanguageDE();
                case LanguageEnum.Korean:
                    return new LanguageKR();
                case LanguageEnum.Portuguese:
                    return new LanguagePT();
                case LanguageEnum.Russian:
                    return new LanguageRU();
                case LanguageEnum.Spanish:
                    return new LanguageES();
                case LanguageEnum.Thai:
                    return new LanguageTH();
                default:
                    return new LanguageEN(); // English by default.
            }
        }
    }
}
