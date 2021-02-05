using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Mock
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazorMock(this IServiceCollection services)
        {
            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
