using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Database;
using Sidekick.Localization;

namespace Sidekick
{
    public static class Startup
    {
        public static ServiceProvider InitializeServices(App application)
        {
            var services = new ServiceCollection()
              .AddSidekickConfiguration()
              .AddSidekickCoreServices()
              .AddSidekickBusinessServices()
              .AddSidekickLocalization()
              .AddSidekickUIWindows()
              .AddSidekickDatabase()
              .AddLocalization()
              .AddMediatR(
                typeof(Business.StartupExtensions),
                typeof(Core.StartupExtensions));

            services.AddSingleton(application);
            services.AddSingleton<INativeApp, App>((sp) => sp.GetRequiredService<App>());

            return services.BuildServiceProvider();
        }
    }
}
