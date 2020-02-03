using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sidekick.Localization
{
    public interface IUILanguageProvider
    {
        CultureInfo Current { get; }

        List<CultureInfo> AvailableLanguages { get; }

        event Action UILanguageChanged;

        void SetLanguage(string name);
    }
}
