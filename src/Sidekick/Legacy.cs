using System;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Languages;
using Sidekick.Core.Loggers;

namespace Sidekick
{
    [Obsolete]
    public static class Legacy
    {
        [Obsolete]
        public static ILogger Logger { get; private set; }

        [Obsolete]
        public static ILanguageProvider LanguageProvider { get; private set; }

        [Obsolete]
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetService<ILogger>();
            LanguageProvider = serviceProvider.GetService<ILanguageProvider>();
        }
    }
}
