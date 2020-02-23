using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sidekick.Localization
{
    public interface IUILanguageProvider
    {
        List<CultureInfo> AvailableLanguages { get; }

        void SetLanguage(string name);
    }
}
