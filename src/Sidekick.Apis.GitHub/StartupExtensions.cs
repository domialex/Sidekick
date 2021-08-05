using Microsoft.Extensions.DependencyInjection;
using Sidekick.Apis.GitHub.Localization;

namespace Sidekick.Apis.GitHub
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickGitHubApi(this IServiceCollection services)
        {
            services.AddTransient<UpdateResources>();
            services.AddTransient<IGitHubClient, GitHubClient>();

            return services;
        }
    }
}
