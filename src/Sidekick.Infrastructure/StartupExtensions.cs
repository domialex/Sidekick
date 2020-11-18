using Microsoft.Extensions.DependencyInjection;
using Sidekick.Infrastructure.Github;
using Sidekick.Infrastructure.PoeApi;
using Sidekick.Infrastructure.PoePriceInfo;

namespace Sidekick.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IPoeTradeClient, PoeTradeClient>();
            services.AddTransient<IGithubClient, GithubClient>();
            services.AddTransient<IPoePriceInfoClient, PoePriceInfoClient>();

            return services;
        }
    }
}
