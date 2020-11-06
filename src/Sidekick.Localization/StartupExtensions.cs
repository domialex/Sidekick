using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();

            return services;
        }
    }
}
