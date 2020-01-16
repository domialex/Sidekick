using Sidekick.Business.Languages.Implementations.UI;
using Sidekick.Business.Loggers;
using Sidekick.Core.DependencyInjection.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    [SidekickService(typeof(IUILanguageProvider))]
    public class UILanguageProvider : IUILanguageProvider
    {
        private readonly ILogger logger;

        public UILanguageProvider(ILogger logger)
        {
            this.logger = logger;
            UILanguage = new UILanguageEN();
            Current = UILanguageEnum.English;

            UILanguageChanged = new List<Action>();
        }

        public UILanguageEnum Current { get; private set; }
        public IUILanguage UILanguage { get; private set; }

        public List<Action> UILanguageChanged { get; private set; }

        public void SetUILanguageProvider(UILanguageEnum language)
        {
            var lang = GetLanguage(language);
            logger.Log($"Changed UI language to {language.ToString()}.");
            Current = language;
            UILanguage = lang;

            if(UILanguageChanged != null)
            {
                foreach(var item in UILanguageChanged)
                {
                    item.Invoke();
                }
            }
        }

        public static IUILanguage GetLanguage(UILanguageEnum language)
        {
            switch(language)
            {
                case UILanguageEnum.English:
                    return new UILanguageEN();
                case UILanguageEnum.German:
                    return new UILanguageDE();
                default:
                    return new UILanguageEN();  // English by default
            }
        }
    }
}
