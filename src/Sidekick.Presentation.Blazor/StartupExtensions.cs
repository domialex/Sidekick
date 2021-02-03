using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Blazor.Settings;
using Sidekick.Presentation.Blazor.About;
using Sidekick.Presentation.Blazor.Overlays.MapInfo;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<AboutModel>();
            services.AddScoped<MapInfoModel>();

            return services;
        }
    }
}
