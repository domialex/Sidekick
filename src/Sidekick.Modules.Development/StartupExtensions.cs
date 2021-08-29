using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Blazor;
using Sidekick.Common.Blazor.Views;
using Sidekick.Development.Views;

namespace Sidekick.Modules.Development
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickDevelopment(this IServiceCollection services)
        {
            services.AddSidekickModule(new SidekickModule()
            {
                Assembly = typeof(StartupExtensions).Assembly
            });

            services.AddScoped<IViewInstance, DevelopmentViewInstance>();

            return services;
        }
    }
}
