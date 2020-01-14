using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.DependencyInjection.Configuration;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.DependencyInjection.Startup;
using System;

namespace Sidekick.Core
{
  public static class Startup
  {
    public static IServiceProvider InitializeServices()
    {
      var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

      var services = new ServiceCollection()
        .AddSidekickConfiguration(configuration)
        .AddSidekickServices()
        .AddSidekickStartup(configuration);

      return services.BuildServiceProvider();
    }
  }
}
