using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;

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
              .AddSidekickConfiguration(configuration)
              .AddSidekickServices();

            return services.BuildServiceProvider();
        }
    }
}
