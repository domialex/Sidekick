using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Sidekick.Presentation.Blazor.Localization;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddLocalization();

            services.AddTransient<ErrorResources>();
            services.AddTransient<InitializationResources>();
            services.AddTransient<TrayResources>();

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
