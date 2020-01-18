using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;
using System;

namespace Sidekick
{
    public static class Startup
    {
        public static IServiceProvider InitializeServices()
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
