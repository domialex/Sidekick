using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Initialization;
using Sidekick.Business;
using Sidekick.Core;
using Sidekick.Database;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mediator;
using Sidekick.Presentation.App;
using Sidekick.Presentation.Initialization.Commands;

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
                    typeof(Infrastructure.StartupExtensions).Assembly,
                    typeof(Business.StartupExtensions).Assembly,
                    typeof(Core.StartupExtensions).Assembly,
                    typeof(Localization.StartupExtensions).Assembly,
                    typeof(StartupExtensions).Assembly,
                    typeof(SetupHandler).Assembly
                )

                .AddSidekickConfiguration()
                .AddSidekickCoreServices()
                .AddSidekickBusinessServices()
                .AddSidekickLocalization()
                .AddSidekickInfrastructure()
                .AddSidekickUIWindows()
                .AddSidekickDatabase();

            services.AddSingleton(application);
            services.AddSingleton(application.Dispatcher);
            services.AddSingleton<INativeApp, App>((sp) => sp.GetRequiredService<App>());

            return services.BuildServiceProvider();
        }
    }
}
