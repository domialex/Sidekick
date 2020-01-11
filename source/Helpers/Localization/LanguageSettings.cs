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
        public static IUILanguageProvider UIProvider { get; private set; } = new UILanguageProviderEN();        // Default to english language
        public static Language CurrentClientLanguage { get; private set; } = Language.English;
        public static Language CurrentUILanguage { get; private set; } = Language.English;

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

        private static Dictionary<Language, IUILanguageProvider> _uiProviderDictionary = new Dictionary<Language, IUILanguageProvider>()
        {
            { Language.English, new UILanguageProviderEN() },
            { Language.German, new UILanguageProvidersDE() },
        };

        private static void ChangeLanguage(Language lang)       // Maybe async?
        {
            if(_providerDictionary.ContainsKey(lang))
            {
                Provider = _providerDictionary[lang];
                CurrentClientLanguage = lang;
                TradeClient.FetchAPIData().Wait();
            }
            else
            {
                throw new Exception("Language not implemented yet");
            }
        }

        public static void ChangeUILanguage(Language lang)
        {
            if(_uiProviderDictionary.ContainsKey(lang))
            {
                UIProvider = _uiProviderDictionary[lang];
                CurrentUILanguage = lang;
            }
            else
            {
                throw new Exception("UI Language not implemented yet");
            }
        }

        public static void DetectLanguage(string input)
        {
            if(input.Contains(_providerDictionary[Language.English].DescriptionRarity))
            {
                if(CurrentClientLanguage != Language.English)
                {
                    ChangeLanguage(Language.English);
                }
            }
            else if(input.Contains(_providerDictionary[Language.German].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.German)
                {
                    ChangeLanguage(Language.German);
                }
            }
            else if(input.Contains(_providerDictionary[Language.French].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.French)
                {
                    ChangeLanguage(Language.French);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Korean].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.Korean)
                {
                    ChangeLanguage(Language.Korean);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Portuguese].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.Portuguese)
                {
                    ChangeLanguage(Language.Portuguese);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Russian].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.Russian)
                {
                    ChangeLanguage(Language.Russian);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Spanish].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.Spanish)
                {
                    ChangeLanguage(Language.Spanish);
                }
            }
            else if(input.Contains(_providerDictionary[Language.Thai].DescriptionRarity))
            {
                if (CurrentClientLanguage != Language.Thai)
                {
                    ChangeLanguage(Language.Thai);
                }
            }

            // Do nothing if random text is copied
        }
    }
}
