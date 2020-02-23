using System;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Languages;

namespace Sidekick
{
    [Obsolete]
    public static class Legacy
    {
        [Obsolete]
        public static ILanguageProvider LanguageProvider { get; private set; }

        [Obsolete]
        public static void Initialize(IServiceProvider serviceProvider)
        {
            LanguageProvider = serviceProvider.GetService<ILanguageProvider>();
        }
    }
}
