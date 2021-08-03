using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Sidekick.Presentation.Blazor.Debounce;
using Sidekick.Presentation.Blazor.Initialization;

namespace Sidekick.Presentation.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazor(this IServiceCollection services)
        {
            services.AddSingleton<InitializationViewModel>();
            services.AddSingleton<IDebouncer, Debouncer>();

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
