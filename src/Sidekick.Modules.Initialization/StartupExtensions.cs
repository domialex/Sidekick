using Microsoft.Extensions.DependencyInjection;
using Sidekick.Modules.Initialization.Localization;

namespace Sidekick.Modules.Initialization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInitialization(this IServiceCollection services)
        {
            services.AddTransient<InitializationResources>();

            return services;
        }
    }
}
