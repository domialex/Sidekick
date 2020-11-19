using System;
using System.Net.Http;
using System.Text.Json;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public class PoePriceInfoClient : IPoePriceInfoClient
    {
        public PoePriceInfoClient(IHttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.CreateClient();
            Client.BaseAddress = new Uri("https://www.poeprices.info/api");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
            Client.DefaultRequestHeaders.UserAgent.TryParseAdd("Sidekick");
            Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public JsonSerializerOptions Options { get; }

        public HttpClient Client { get; }
    }
}
