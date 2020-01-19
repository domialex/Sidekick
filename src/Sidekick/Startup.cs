using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business;
using Sidekick.Core;

namespace Sidekick
{
    public static class Startup
    {
        public static ServiceProvider InitializeServices()
        {
            var services = new ServiceCollection()
              .AddSidekickConfiguration()
              .AddSidekickCoreServices()
              .AddSidekickBusinessServices()
              .AddSidekickServices();

            return services.BuildServiceProvider();
        }
    }
}
