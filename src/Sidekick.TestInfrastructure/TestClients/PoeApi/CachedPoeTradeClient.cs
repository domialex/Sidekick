using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade;

namespace Sidekick.TestInfrastructure.TestClients.PoeApi
{
    class CachedPoeTradeClient : IPoeTradeClient
    {
        private readonly PoeTradeClient innerClient;

        public CachedPoeTradeClient(PoeTradeClient innerClient)
        {
            this.innerClient = innerClient;
        }

        public JsonSerializerOptions Options => innerClient.Options;

        public async Task<List<TReturn>> Fetch<TReturn>()
        {
            var fileName = Path.Combine(Assembly.GetExecutingAssembly().Location, "TestClients", "PoeApi", $"{typeof(TReturn).Name}.json");

            if (File.Exists(fileName))
            {
                return JsonSerializer.Deserialize<List<TReturn>>(File.ReadAllText(fileName), Options);
            }

            var response = await innerClient.Fetch<TReturn>();

            File.WriteAllText(fileName, JsonSerializer.Serialize(response));

            return response;
        }
    }
}
