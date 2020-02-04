using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Sidekick.Core.Settings;

namespace Sidekick.Localization
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private static string[] SupportedLanguages = new[] { "en", "fr", "de" };

        public event Action UILanguageChanged;

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

        public CultureInfo Current { get; private set; }

        public void SetLanguage(string name)
        {
            Current = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = Current;
            Thread.CurrentThread.CurrentUICulture = Current;

            if (UILanguageChanged != null)
            {
                UILanguageChanged.Invoke();
            }
        }
    }
}
