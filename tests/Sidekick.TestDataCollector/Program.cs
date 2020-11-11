using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sidekick.Application;
using Sidekick.Business;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Process;
using Sidekick.Infrastructure;
using Sidekick.Mediator;

namespace Sidekick.TestDataCollector
{
    public static class Program
    {
        // Collects required data from live APIs and stores it for use in testing.
        // Needs to be run when API data may have been update, like when new leagues are released.
        // When tests are added which rely on data other than ItemDataCategory or StatDataCategory, this program will have to be updated.
        public static async Task Main()
        {
            var services = new ServiceCollection()
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Business")
                )

                .AddSingleton(new Mock<INativeProcess>().Object)
                .AddSidekickApplication()
                .AddSidekickBusinessServices()
                .AddSidekickInfrastructure();


            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();
            await mediator.Send(new InitializeCommand(true));

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
