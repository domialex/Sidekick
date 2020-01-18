using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;

namespace Sidekick.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IInitializeService, InitializeService>();

            return services;
        }
    }
}
