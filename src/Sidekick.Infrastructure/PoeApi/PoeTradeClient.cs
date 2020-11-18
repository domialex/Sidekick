using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Languages;

namespace Sidekick.Infrastructure.PoeApi
{
    public class PoeTradeClient : IPoeTradeClient
    {
        private readonly ILogger logger;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly HttpClient client;

        public PoeTradeClient(
            ILogger<PoeTradeClient> logger,
            IGameLanguageProvider gameLanguageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.gameLanguageProvider = gameLanguageProvider;
            client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("Sidekick");

            Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };
            Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public JsonSerializerOptions Options { get; }

        public async Task<List<TReturn>> Fetch<TReturn>(string path, bool useDefaultLanguage = false)
        {
            var name = typeof(TReturn).Name;
            var language = useDefaultLanguage ? gameLanguageProvider.EnglishLanguage : gameLanguageProvider.Language;

            try
            {
                var response = await client.GetAsync(language.PoeTradeApiBaseUrl + path);
                var content = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FetchResult<TReturn>>(content, Options);
                return result.Result;
            }
            catch (Exception)
            {
                logger.LogInformation($"Could not fetch {name} at {language.PoeTradeApiBaseUrl + path}.");
                throw;
            }
        }
    }
}
