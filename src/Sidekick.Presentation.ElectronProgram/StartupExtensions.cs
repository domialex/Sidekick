using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Views;
using Sidekick.Presentation.ElectronProgram.Views;

namespace Sidekick.Presentation.ElectronProgram
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationElectron(this IServiceCollection services)
        {
            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
