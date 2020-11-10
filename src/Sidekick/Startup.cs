using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application;
using Sidekick.Application.Initialization;
using Sidekick.Business;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mediator;
using Sidekick.Persistence;
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
                    typeof(Localization.StartupExtensions).Assembly,
                    typeof(StartupExtensions).Assembly,
                    typeof(SetupHandler).Assembly
                )

                .AddSidekickApplication()
                .AddSidekickBusinessServices()
                .AddSidekickLocalization()
                .AddSidekickInfrastructure()
                .AddSidekickUIWindows()
                .AddSidekickDatabase();

            services.AddSingleton(application);
            services.AddSingleton(application.Dispatcher);

            return services.BuildServiceProvider();
        }
    }
}
