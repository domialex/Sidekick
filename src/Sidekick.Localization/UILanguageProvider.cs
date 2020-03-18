using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Localization
{
    public class UILanguageProvider : IUILanguageProvider, IOnInit
    {
        private static readonly string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };
        private readonly SidekickSettings settings;

        public UILanguageProvider(SidekickSettings settings)
        {
            AvailableLanguages = SupportedLanguages
                .Select(x => new CultureInfo(x))
                .ToList();

            this.settings = settings;
        }

        public List<CultureInfo> AvailableLanguages { get; private set; }

        public Task OnInit()
        {
            var current = AvailableLanguages.FirstOrDefault(x => x.Name == settings.Language_UI);
            if (current != null)
            {
                SetLanguage(settings.Language_UI);
            }
            else
            {
                SetLanguage(SupportedLanguages.FirstOrDefault());
            }

            return Task.CompletedTask;
        }

        public void SetLanguage(string name)
        {
            TranslationSource.Instance.CurrentCulture = new CultureInfo(name);
        }
    }
}
