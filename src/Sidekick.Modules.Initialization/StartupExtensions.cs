using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Modules.Initialization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInitialization(this IServiceCollection services)
        {


            return services;
        }
    }
}
