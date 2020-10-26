using Microsoft.Extensions.DependencyInjection;
using Sidekick.Infrastructure.Github;

namespace Sidekick.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IGithubClient, GithubClient>();

            return services;
        }
    }
}
