using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Mock
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationBlazorMock(this IServiceCollection services)
        {
            services.AddSingleton<IViewLocator, ViewLocator>();
            services.AddSingleton<IKeybindProvider, KeybindProvider>();

            return services;
        }
    }
}
