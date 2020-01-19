using Sidekick.Business.Languages.UI.Implementations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages.UI
{
    public interface IUILanguageProvider
    {
        IUILanguage Language { get; }

        UILanguageAttribute Current { get; }

        List<UILanguageAttribute> AvailableLanguages { get; }

        event Func<Task> UILanguageChanged;

        Task SetLanguage(UILanguageAttribute language);
    }
}
