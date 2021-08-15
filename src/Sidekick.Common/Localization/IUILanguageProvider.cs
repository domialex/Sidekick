using System.Collections.Generic;
using System.Globalization;

namespace Sidekick.Common.Localization
{
    public interface IUILanguageProvider
    {
        /// <summary>
        /// Gets the list of available UI languages
        /// </summary>
        List<CultureInfo> GetList();

        /// <summary>
        /// Sets the Ui language
        /// </summary>
        /// <param name="name">The culture name of the desired language</param>
        void Set(string name);
    }
}
