using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sidekick.Core.Settings;

namespace Sidekick.Localization
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private static string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };

        public UILanguageProvider(SidekickSettings settings)
        {
            AvailableLanguages = SupportedLanguages
                .Select(x => new CultureInfo(x))
                .ToList();

            var current = AvailableLanguages.FirstOrDefault(x => x.Name == settings.Language_UI);
            if (current != null)
            {
                SetLanguage(settings.Language_UI);
            }
            else
            {
                SetLanguage(SupportedLanguages.FirstOrDefault());
            }
        }

        public List<CultureInfo> AvailableLanguages { get; private set; }

        public void SetLanguage(string name)
        {
            TranslationSource.Instance.CurrentCulture = new CultureInfo(name);
        }
    }
}
