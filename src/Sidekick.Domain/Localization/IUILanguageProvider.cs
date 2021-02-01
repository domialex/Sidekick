using System.Collections.Generic;
using System.Globalization;

namespace Sidekick.Domain.Localization
{
    public interface IUILanguageProvider
    {
        List<CultureInfo> AvailableLanguages { get; }

        void SetLanguage(string name);
    }
}
