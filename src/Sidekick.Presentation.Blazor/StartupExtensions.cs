using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Blazor.Settings;
using Sidekick.Presentation.Blazor.About;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<AboutViewModel>();

            return services;
        }
    }
}
