using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Sidekick.Domain.Localization;

namespace Sidekick.Presentation.Localization
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private static readonly string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };

        public UILanguageProvider()
        {
            AvailableLanguages = SupportedLanguages
                .Select(x => CultureInfo.GetCultureInfo(x))
                .ToList();
        }

        public List<CultureInfo> AvailableLanguages { get; private set; }

        public void SetLanguage(string name)
        {
            TranslationSource.Instance.CurrentCulture = CultureInfo.GetCultureInfo(name);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(name);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(name);
        }
    }
}
