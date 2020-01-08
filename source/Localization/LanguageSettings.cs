using Sidekick.Helpers.POETradeAPI;
using Sidekick.Localization.Languages;
using Sidekick.Localization.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization
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
        Korean
    };

    public static class LanguageSettings
    {
        private static LanguageProvider _languageProvider = new LanguageProviderEN();       // Default to english language
        private static Language _currentLanguage = Language.English;
        private static Dictionary<Language, LanguageProvider> _typeDictionary = new Dictionary<Language, LanguageProvider>()
        {
            { Language.English, new LanguageProviderEN() },
            { Language.German, new LanguageProviderDE() },
            { Language.French, new LanguageProviderFR() },
            { Language.Portuguese, new LanguageProviderPT() },
            { Language.Russian, new LanguageProviderRU() },
            { Language.Spanish, new LanguageProviderES() },
            { Language.Korean, new LanguageProviderKR() },
            { Language.Thai, new LanguageProviderTH() },
        };

        public static LanguageProvider Provider => _languageProvider;

        public static void ChangeLanguage(Language lang)        // Maybe make method async
        {
            if(_typeDictionary.ContainsKey(lang))
            {
                _languageProvider = _typeDictionary[lang];
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
            if(input.Contains(StringConstantsEN.DescriptionRarity))
            {
                if(_currentLanguage != Language.English)        // Switch on language change
                {
                    ChangeLanguage(Language.English);
                }
            }
            else if(input.Contains(StringConstantsDE.DescriptionRarity))
            {
                if(_currentLanguage != Language.German)
                {
                    ChangeLanguage(Language.German);
                }
            }
            else if(input.Contains(StringConstantsFR.DescriptionRarity))
            {
                if(_currentLanguage != Language.French)
                {
                    ChangeLanguage(Language.French);
                }
            }
            else if(input.Contains(StringConstantsPT.DescriptionRarity))
            {
                if(_currentLanguage != Language.Portuguese)
                {
                    ChangeLanguage(Language.Portuguese);
                }
            }
            else if(input.Contains(StringConstantsRU.DescriptionRarity))
            {
                if(_currentLanguage != Language.Russian)
                {
                    ChangeLanguage(Language.Russian);
                }
            }
            else if(input.Contains(StringConstantsES.DescriptionRarity))
            {
                if(_currentLanguage != Language.Spanish)
                {
                    ChangeLanguage(Language.Spanish);
                }
            }
            else if(input.StartsWith(StringConstantsKR.DescriptionRarity))
            {
                if(_currentLanguage != Language.Korean)
                {
                    ChangeLanguage(Language.Korean);
                }
            }
            else if(input.StartsWith(StringConstantsTH.DescriptionRarity))
            {
                if(_currentLanguage != Language.Thai)
                {
                    ChangeLanguage(Language.Thai);
                }
            }
            else // Do nothing if radnom text is copied
            {
                //throw new NotImplementedException();
            }
        }
    }
}
