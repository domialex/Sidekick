using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Initialization;
using Sidekick.Business;
using Sidekick.Core;
using Sidekick.Database;
using Sidekick.Domain.Natives.App;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mediator;

namespace Sidekick
{
    public static class Startup
    {
        public static ServiceProvider InitializeServices(App application)
        {
            var services = new ServiceCollection()

                // Building blocks
                .AddSidekickLogging()
                .AddSidekickMediator(
                    typeof(InitializeHandler).Assembly,
                    typeof(Business.StartupExtensions).Assembly,
                    typeof(Core.StartupExtensions).Assembly,
                    typeof(Localization.StartupExtensions).Assembly,
                    typeof(Sidekick.StartupExtensions).Assembly
                )

                .AddSidekickConfiguration()
                .AddSidekickCoreServices()
                .AddSidekickBusinessServices()
                .AddSidekickLocalization()
                .AddSidekickUIWindows()
                .AddSidekickDatabase();

            services.AddSingleton(application);
            services.AddSingleton(application.Dispatcher);
            services.AddSingleton<INativeApp, App>((sp) => sp.GetRequiredService<App>());

            return services.BuildServiceProvider();
        }
    }
}
