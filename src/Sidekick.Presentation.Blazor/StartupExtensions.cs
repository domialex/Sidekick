using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Blazor.About;
using Sidekick.Presentation.Blazor.Initialization;
using Sidekick.Presentation.Blazor.Overlays.MapInfo;
using Sidekick.Presentation.Blazor.Settings;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddSingleton<InitializationViewModel>();

            services.AddScoped<SettingsViewModel>();
            services.AddScoped<AboutModel>();
            services.AddScoped<MapInfoModel>();

            return services;
        }
    }
}
