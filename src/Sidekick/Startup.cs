using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business;
using Sidekick.Core;

namespace Sidekick
{
    public static class Startup
    {
        public static ServiceProvider InitializeServices()
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true)
              .Build();

            var services = new ServiceCollection()
              .AddSidekickCoreServices()
              .AddSidekickBusinessServices()
              .AddSidekickServices();

            return services.BuildServiceProvider();
        }
    }
}
