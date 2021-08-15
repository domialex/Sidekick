using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sidekick.Common.Localization
{
    public class UILanguageProvider : IUILanguageProvider
    {
        private static readonly string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };

        public List<CultureInfo> GetList()
        {
            var languages = SupportedLanguages
                .Select(x => CultureInfo.GetCultureInfo(x))
                .ToList();
            return languages;
        }

        public void Set(string name)
        {
            var languages = GetList();

            if (!languages.Any(x => x.Name == name))
            {
                name = languages.FirstOrDefault()?.Name;
            }

            if (!string.IsNullOrEmpty(name))
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(name);
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(name);
            }
        }
    }
}
