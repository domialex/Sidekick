using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business;
using Sidekick.Core;
using Sidekick.Platforms.Windows;
using Sidekick.UI;

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
              .AddSidekickServices()
              .AddSidekickWindowsServices()
              .AddSidekickUIServices()
              .AddSidekickUIWindows()
              .AddPoeNinja();

            return services.BuildServiceProvider();
        }
    }
}
