using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Common.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickModule(this IServiceCollection services, SidekickModule module)
        {
            SidekickModule.Modules.Add(module);

            return services;
        }
    }
}
