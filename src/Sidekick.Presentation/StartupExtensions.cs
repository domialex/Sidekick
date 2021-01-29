using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Debounce;
using Sidekick.Presentation.Localization;

namespace Sidekick.Presentation
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentation(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();
            services.AddSingleton<IDebouncer, Debouncer>();

            return services;
        }
    }
}
