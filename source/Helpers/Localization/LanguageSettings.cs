using Sidekick.Helpers.POETradeAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Helpers.Localization
{
    public enum Language
    {
        English,
        French,
        German,
        Korean,
        Portuguese,
        Russian,
        Spanish,
        Thai,
        TraditionalChinese,
    };

    public static class LanguageSettings
    {
        public static ILanguageProvider Provider { get; private set; } = new LanguageProviderEN();
        public static Language CurrentLanguage { get; private set; } = Language.English;

        private static Dictionary<string, Language> RarityToLanguageDictionary = new Dictionary<string, Language>()
        {
            { "Rarity: ", Language.English },
            { "Rareté: ", Language.French },
            { "Seltenheit: ", Language.German },
            { "아이템 희귀도: ", Language.Korean },
            { "Raridade: ", Language.Portuguese },
            { "Редкость: ", Language.Russian },
            { "Rareza: ", Language.Spanish },
            { "ความหายาก: ", Language.Thai },
            { "稀有度: ", Language.TraditionalChinese },
        };

        /// <summary>
        /// Every item should start with Rarity in the first line. 
        /// This will force the TradeClient to refetch the Public API's data if needed.
        /// </summary>
        public static async Task<bool> FindAndSetLanguageProvider(string itemDescription)
        {
            foreach (var item in RarityToLanguageDictionary)
            {
                if (itemDescription.Contains(item.Key))
                {
                    if (CurrentLanguage != item.Value)
                    {
                        Logger.Log($"Changed language support to {item.Value}.");
                        CurrentLanguage = item.Value;
                        Provider = GetLanguageProvider(item.Value);
                        TradeClient.Dispose();
                        return await TradeClient.Initialize();
                    }

                    return true;
                }
            }

            Logger.Log("This Path of Exile language is not yet supported.");
            return false;
        }

        public static ILanguageProvider GetLanguageProvider(Language language)
        {
            switch (language)
            {
                case Language.French:
                    return new LanguageProviderFR();
                case Language.German:
                    return new LanguageProviderDE();
                case Language.Korean:
                    return new LanguageProviderKR();
                case Language.Portuguese:
                    return new LanguageProviderPT();
                case Language.Russian:
                    return new LanguageProviderRU();
                case Language.Spanish:
                    return new LanguageProviderES();
                case Language.Thai:
                    return new LanguageProviderTH();
                case Language.TraditionalChinese:
                    return new LanguageProviderZHTW();
                default:
                    return new LanguageProviderEN(); // English by default.
            }
        }
    }
}
