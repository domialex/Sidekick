using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application;
using Sidekick.Business;
using Sidekick.Infrastructure;
using Sidekick.Logging;
using Sidekick.Mediator;
using Sidekick.Persistence;

namespace Sidekick.Presentation.Wpf
{
    public static class Startup
    {
        public static ServiceProvider InitializeServices(App application)
        {
            var services = new ServiceCollection()

                // Building blocks
                .AddSidekickLogging()
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"),
                    Assembly.Load("Sidekick.Presentation"),
                    Assembly.Load("Sidekick.Presentation.Wpf"),
                    Assembly.Load("Sidekick.Business")
                )

                .AddSidekickApplication()
                .AddSidekickBusinessServices()
                .AddSidekickInfrastructure()
                .AddSidekickPersistence()
                .AddSidekickPresentation()
                .AddSidekickPresentationWpf();

            services.AddSingleton(application);
            services.AddSingleton(application.Dispatcher);

            return services.BuildServiceProvider();
        }
    }
}
