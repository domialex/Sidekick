using Microsoft.Extensions.DependencyInjection;
using Sidekick.Presentation.Debounce;

namespace Sidekick.Presentation
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentation(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<IDebouncer, Debouncer>();

            return services;
        }
    }
}
