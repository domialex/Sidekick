using Microsoft.Extensions.DependencyInjection;
using Sidekick.Infrastructure.Github;
using Sidekick.Infrastructure.Poe;

namespace Sidekick.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IPoeTradeClient, PoeTradeClient>();
            services.AddTransient<IGithubClient, GithubClient>();

            return services;
        }
    }
}
