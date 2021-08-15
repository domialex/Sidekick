using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Platform;

namespace Sidekick.Mock
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickMocks(this IServiceCollection services)
        {
            services.AddSingleton<IAppService, MockAppService>();
            services.AddSingleton<IKeyboardProvider, MockKeyboardProvider>();
            services.AddSingleton<IProcessProvider, MockProcessProvider>();
            services.AddSingleton<IViewLocator, MockViewLocator>();
            services.AddScoped<IViewInstance, MockViewInstance>();
            services.AddSingleton<IKeybindProvider, MockKeybindProvider>();

            return services;
        }
    }
}
