using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddInitializableService<IUILanguageProvider, UILanguageProvider>();

            return services;
        }
    }
}
