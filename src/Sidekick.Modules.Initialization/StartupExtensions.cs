using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Blazor;
using Sidekick.Modules.Initialization.Localization;

namespace Sidekick.Modules.Initialization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInitialization(this IServiceCollection services)
        {
            services.AddTransient<InitializationResources>();

            services.AddSidekickModule(new SidekickModule()
            {
                Assembly = typeof(StartupExtensions).Assembly
            });

            return services;
        }
    }
}
