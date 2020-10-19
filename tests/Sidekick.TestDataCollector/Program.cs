using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sidekick.Business;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Core;
using Sidekick.Core.Initialization;
using Sidekick.Core.Mediator;
using Sidekick.Core.Natives;
using Sidekick.Database;
using Sidekick.Localization;

namespace Sidekick.TestDataCollector
{
    public static class Program
    {
        // Collects required data from live APIs and stores it for use in testing.
        // Needs to be run when API data may have been update, like when new leagues are released.
        // When tests are added which rely on data other than ItemDataCategory or StatDataCategory, this program will have to be updated.
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection()
              .AddSingleton<INativeApp>(new Mock<INativeApp>().Object)
              .AddSingleton<INativeNotifications>(new Mock<INativeNotifications>().Object)
              .AddSingleton<INativeBrowser>(new Mock<INativeBrowser>().Object)
              .AddSingleton<INativeProcess>(new Mock<INativeProcess>().Object)
              .AddSidekickConfiguration()
              .AddSidekickCoreServices()
              .AddSidekickBusinessServices()
              .AddSidekickLocalization()
              .AddSidekickDatabase()
              .AddSingleton(typeof(IPipelineBehavior<,>), typeof(MediatorLoggingBehavior<,>))
              .AddMediatR(
                (config) => config.Using<SidekickMediator>().AsTransient(),
                typeof(Business.StartupExtensions),
                typeof(Core.StartupExtensions),
                typeof(Localization.StartupExtensions));


            var provider = services.BuildServiceProvider();

            var initializer = provider.GetRequiredService<IInitializer>();
            await initializer.Initialize(true);

            var client = provider.GetRequiredService<IPoeTradeClient>();

            await FetchAndSaveData<ItemDataCategory>(client);
            await FetchAndSaveData<StatDataCategory>(client);
        }

        private static async Task FetchAndSaveData<T>(IPoeTradeClient client)
        {
            var data = await client.Fetch<T>();

            File.WriteAllText(Path.Join(
                "..", "..", "..", "..",
                "Sidekick.TestInfrastructure",
                "TestData",
                $"{typeof(T).Name}.json"),
                JsonSerializer.Serialize(data, client.Options));
        }
    }
}
