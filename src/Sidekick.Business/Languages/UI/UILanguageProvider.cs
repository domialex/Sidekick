using Sidekick.Business.Languages.UI.Implementations;
using Sidekick.Core.Extensions;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages.UI
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private readonly ILogger logger;

        public UILanguageProvider(ILogger logger)
        {
            this.logger = logger;
            Language = new UILanguageEN();

            AvailableLanguages = new List<UILanguageAttribute>();
            foreach (var type in typeof(UILanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<UILanguageAttribute>();
                attribute.ImplementationType = type;
                AvailableLanguages.Add(attribute);
            }

            Language = new UILanguageEN();
        }

        public List<UILanguageAttribute> AvailableLanguages { get; private set; }

        public UILanguageAttribute Current => AvailableLanguages.First(x => x.Name == Language.LanguageName);

        public IUILanguage Language { get; private set; }

        public event Action UILanguageChanged;

        public void SetLanguage(UILanguageAttribute language)
        {
            if (language.Name != Current.Name)
            {
                logger.Log($"Changed UI language to {language.Name}.");
                Language = (IUILanguage)Activator.CreateInstance(language.ImplementationType);
                NotifyLanguageSet();
            }
        }

        private void NotifyLanguageSet()
        {
            if (UILanguageChanged != null)
            {
                foreach (var handler in UILanguageChanged.GetInvocationList())
                {
                    ((Action)handler)();
                }
            }
        }
    }
}
