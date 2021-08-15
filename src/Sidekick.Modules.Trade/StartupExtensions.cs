using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Modules.Trade
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickTrade(this IServiceCollection services)
        {
            return services;
        }
    }
}
