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

            var current = AvailableLanguages.FirstOrDefault(x => x.Name == settings.UILanguage);
            if (current != null)
            {
                SetLanguage(current);
            }
            else
            {
                Language = new UILanguageEN();
            }
        }

        public List<UILanguageAttribute> AvailableLanguages { get; private set; }

        public UILanguageAttribute Current => AvailableLanguages.FirstOrDefault(x => x.Name == Language?.LanguageName);

        public IUILanguage Language { get; private set; }

        public event Action UILanguageChanged;

        public void SetLanguage(UILanguageAttribute language)
        {
            if (language.Name != Current?.Name)
            {
                logger.Log($"Changed UI language to {language.Name}.");
                Language = (IUILanguage)Activator.CreateInstance(language.ImplementationType);
                NotifyLanguageSet();
            }
        }

        private void NotifyLanguageSet()
        {
            if (UILanguageChanged == null)
            {
                return;
            }

            foreach (var handler in UILanguageChanged.GetInvocationList())
            {
                ((Action)handler)();
            }
        }
    }
}
