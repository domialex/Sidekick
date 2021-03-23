using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Sidekick.Presentation.Blazor.Debounce;
using Sidekick.Presentation.Blazor.Initialization;
using Sidekick.Presentation.Blazor.Prices;
using Sidekick.Presentation.Blazor.Settings;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddSingleton<InitializationViewModel>();
            services.AddSingleton<IDebouncer, Debouncer>();

            services.AddScoped<SettingsViewModel>();
            services.AddScoped<PricesModel>();

            // Mudblazor
            services
                .AddMudServices()
                .AddMudBlazorDialog()
                .AddMudBlazorSnackbar()
                .AddMudBlazorResizeListener()
                .AddMudBlazorScrollListener()
                .AddMudBlazorScrollManager()
                .AddMudBlazorJsApi();

            return services;
        }
    }
}
