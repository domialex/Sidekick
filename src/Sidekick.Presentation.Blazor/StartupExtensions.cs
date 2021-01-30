using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Blazor.Settings;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddScoped<SettingsViewModel>();

            return services;
        }
    }
}
