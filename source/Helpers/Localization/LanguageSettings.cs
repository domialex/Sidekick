using Sidekick.Helpers.POETradeAPI;
using System;
using System.Collections.Generic;

namespace Sidekick.Helpers.Localization
{
    public enum Language
    {
        English,
        German,
        French,
        Portuguese,
        Russian,
        Thai,
        Spanish,
        Korean,
    };

    public static class LanguageSettings
    {
        public static ILanguageProvider Provider { get; private set; } = new LanguageProviderEN();      // Default to english language
        public static Language CurrentLanguage { get; private set; } = Language.English;

        private static Dictionary<Language, ILanguageProvider> _providerDictionary = new Dictionary<Language, ILanguageProvider>()
        {
            { Language.English, new LanguageProviderEN() },
            { Language.German, new LanguageProviderDE() },
            { Language.French, new LanguageProviderFR() },
            { Language.Portuguese, new LanguageProviderPT() },
            { Language.Russian, new LanguageProviderRU() },
            { Language.Thai, new LanguageProviderTH() },
            { Language.Spanish, new LanguageProviderES() },
            { Language.Korean, new LanguageProviderKR() },
        };

        private static void ChangeLanguage(Language lang)       // Maybe async?
        {
            if (_providerDictionary.ContainsKey(lang))
            {
                Provider = _providerDictionary[lang];
                CurrentLanguage = lang;
                TradeClient.FetchAPIData().Wait();
            }
            else
            {
                throw new Exception("Language not implemented yet");
            }
        }

        public static void DetectLanguage(string input)
        {
            if (input.Contains(_providerDictionary[Language.English].DescriptionRarity))
            {
                if (CurrentLanguage != Language.English)
                {
                    ChangeLanguage(Language.English);
                }
            }
            else if (input.Contains(_providerDictionary[Language.German].DescriptionRarity))
            {
                if (CurrentLanguage != Language.German)
                {
                    ChangeLanguage(Language.German);
                }
            }
            else if (input.Contains(_providerDictionary[Language.French].DescriptionRarity))
            {
                if (CurrentLanguage != Language.French)
                {
                    ChangeLanguage(Language.French);
                }
            }
            else if (input.Contains(_providerDictionary[Language.Korean].DescriptionRarity))
            {
                if (CurrentLanguage != Language.Korean)
                {
                    ChangeLanguage(Language.Korean);
                }
            }
            else if (input.Contains(_providerDictionary[Language.Portuguese].DescriptionRarity))
            {
                if (CurrentLanguage != Language.Portuguese)
                {
                    ChangeLanguage(Language.Portuguese);
                }
            }
            else if (input.Contains(_providerDictionary[Language.Russian].DescriptionRarity))
            {
                if (CurrentLanguage != Language.Russian)
                {
                    ChangeLanguage(Language.Russian);
                }
            }
            else if (input.Contains(_providerDictionary[Language.Spanish].DescriptionRarity))
            {
                if (CurrentLanguage != Language.Spanish)
                {
                    ChangeLanguage(Language.Spanish);
                }
            }
            else if (input.Contains(_providerDictionary[Language.Thai].DescriptionRarity))
            {
                if (CurrentLanguage != Language.Thai)
                {
                    ChangeLanguage(Language.Thai);
                }
            }

            // Do nothing if random text is copied
        }
    }
}
