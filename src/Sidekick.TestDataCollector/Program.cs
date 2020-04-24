using Sidekick.TestInfrastructure;
using AutoFixture;
using Sidekick.Business.Apis.Poe.Trade;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System;

namespace Sidekick.TestDataCollector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fixture = new SidekickFixture();

            var client = fixture.Create<PoeTradeClient>();

            await FetchAndSaveData<ItemDataCategory>(client);
        }

        private static async Task FetchAndSaveData<T>(PoeTradeClient client)
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
