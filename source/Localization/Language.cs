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
        // TODO all Client languages
    };

    public static class LanguageSettings
    {
        private static LanguageProvider _languageProvider = new LanguageProviderEN();       // Default to english language
        private static Language _currentLanguage = Language.English;
        private static Dictionary<Language, LanguageProvider> _typeDictionary = new Dictionary<Language, LanguageProvider>()
        {
            { Language.English, new LanguageProviderEN() },
        };

        public static LanguageProvider Provider => _languageProvider;

        public static void ChangeLanguage(Language lang)
        {
            if(_typeDictionary.ContainsKey(lang))
            {
                _languageProvider = _typeDictionary[lang];
                _currentLanguage = lang;
                TradeClient.UpdateClientBaseUrl(_languageProvider.PoeTradeApiBaseUrl);
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
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
