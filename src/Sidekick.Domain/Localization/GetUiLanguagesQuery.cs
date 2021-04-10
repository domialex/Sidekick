using System.Collections.Generic;
using System.Globalization;
using MediatR;

namespace Sidekick.Domain.Localization
{
    /// <summary>
    /// Gets the list of available UI languages
    /// </summary>
    public class GetUiLanguagesQuery : IQuery<List<CultureInfo>>
    {
        /// <summary>
        /// Gets the list of available UI languages
        /// </summary>
        public GetUiLanguagesQuery()
        {
        }
    }
}
