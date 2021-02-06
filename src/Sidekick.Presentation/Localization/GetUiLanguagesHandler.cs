using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Localization;

namespace Sidekick.Presentation.Localization
{
    public class GetUiLanguagesHandler : IQueryHandler<GetUiLanguagesQuery, List<CultureInfo>>
    {
        private static readonly string[] SupportedLanguages = new[] { "en", "fr", "de", "zh-tw" };

        public Task<List<CultureInfo>> Handle(GetUiLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = SupportedLanguages
                .Select(x => CultureInfo.GetCultureInfo(x))
                .ToList();
            return Task.FromResult(languages);
        }
    }
}
