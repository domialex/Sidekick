using Sidekick.Helpers.POETradeAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static Language _currentLanguage;
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
            if(_providerDictionary.ContainsKey(lang))
            {
                Provider = _providerDictionary[lang];
                _currentLanguage = lang;
                TradeClient.FetchAPIData().Wait();
            }
            else
            {
                throw new Exception("Language not implemented yet");
            }
        }

        public static void DetectLanguage(string input)
        {
            if(input.Contains(_providerDictionary[Language.English].DescriptionRarity))
            {
                if(_currentLanguage != Language.English)
                {
                    ChangeLanguage(Language.English);
                }
            }
            else if(input.Contains(_providerDictionary[Language.German].DescriptionRarity))
            {
                if (_currentLanguage != Language.German)
                {
                    ChangeLanguage(Language.German);
                }
            }
            else if(input.Contains(_providerDictionary[Language.French].DescriptionRarity))
            {
                if (_currentLanguage != Language.French)
                {
                    ChangeLanguage(Language.French);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Korean].DescriptionRarity))
            {
                if (_currentLanguage != Language.Korean)
                {
                    ChangeLanguage(Language.Korean);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Portuguese].DescriptionRarity))
            {
                if (_currentLanguage != Language.Portuguese)
                {
                    ChangeLanguage(Language.Portuguese);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Russian].DescriptionRarity))
            {
                if (_currentLanguage != Language.Russian)
                {
                    ChangeLanguage(Language.Russian);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Spanish].DescriptionRarity))
            {
                if (_currentLanguage != Language.Spanish)
                {
                    ChangeLanguage(Language.Spanish);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Thai].DescriptionRarity))
            {
                if (_currentLanguage != Language.Thai)
                {
                    ChangeLanguage(Language.Thai);
                }
            }

            // Do nothing if random text is copied
        }
    }
}
