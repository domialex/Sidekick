using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sidekick.Localization
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private static readonly string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };

        public UILanguageProvider()
        {
            AvailableLanguages = SupportedLanguages
                .Select(x => new CultureInfo(x))
                .ToList();
        }

        public List<CultureInfo> AvailableLanguages { get; private set; }

        public void SetLanguage(string name)
        {
            TranslationSource.Instance.CurrentCulture = new CultureInfo(name);
        }
    }
}
