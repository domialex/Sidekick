using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Languages.UI.Implementations;
using Sidekick.Core.Extensions;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Languages.UI
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private readonly ILogger logger;

        public UILanguageProvider(ILogger logger,
            SidekickSettings settings)
        {
            this.logger = logger;

            AvailableLanguages = new List<UILanguageAttribute>();
            foreach (var type in typeof(UILanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<UILanguageAttribute>();
                attribute.ImplementationType = type;
                AvailableLanguages.Add(attribute);
            }

            var current = AvailableLanguages.FirstOrDefault(x => x.Name == settings.Language_UI);
            if (current != null)
            {
                SetLanguage(current);
            }
            else
            {
                SetLanguage(AvailableLanguages.OrderBy(x => x.Name == "en" ? 0 : 1).FirstOrDefault());
            }
        }

        public List<UILanguageAttribute> AvailableLanguages { get; private set; }

        public UILanguageAttribute Current { get; private set; }

        public IUILanguage Language { get; private set; }

        public event Action UILanguageChanged;

        public void SetLanguage(UILanguageAttribute language)
        {
            if (language != null && language.DisplayName != Current?.DisplayName)
            {
                logger.Log($"Changed UI language to {language.DisplayName}.");
                Current = language;
                Language = (IUILanguage)Activator.CreateInstance(language.ImplementationType);

                if (UILanguageChanged == null)
                {
                    return;
                }

                UILanguageChanged.Invoke();
            }
        }
    }
}
