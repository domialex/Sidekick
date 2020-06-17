using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.TestInfrastructure;

namespace Sidekick.TestDataCollector
{
    class Program
    {
        // Collects required data from live APIs and stores it for use in testing.
        // Needs to be run when API data may have been update, like when new leagues are released.
        // When tests are added which rely on data other than ItemDataCategory or StatDataCategory, this program will have to be updated.
        static async Task Main(string[] args)
        {
            var fixture = new SidekickFixture();

            var client = fixture.Create<PoeTradeClient>();

            await FetchAndSaveData<ItemDataCategory>(client);
            await FetchAndSaveData<StatDataCategory>(client);
            await FetchAndSaveData<StatDataCategory>(client, true);
        }

        private static async Task FetchAndSaveData<T>(PoeTradeClient client, bool useDefaultLanguage = false)
        {
            var data = await client.Fetch<T>(useDefaultLanguage);

            File.WriteAllText(Path.Join(
                "..", "..", "..", "..",
                "Sidekick.TestInfrastructure",
                "TestData",
                $"{typeof(T).Name}.json"),
                JsonSerializer.Serialize(data, client.Options));
        }
    }
}
