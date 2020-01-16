using Sidekick.Business.Languages.Implementations.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    public interface IUILanguageProvider
    {
        UILanguageEnum Current { get; }
        IUILanguage UILanguage { get; }

        List<Action> UILanguageChanged { get; }

        void SetUILanguageProvider(UILanguageEnum language);
    }
}
