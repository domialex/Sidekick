using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;

namespace Sidekick.Infrastructure.PoeApi
{
    public class PoeTradeClient : IPoeTradeClient
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;
        private readonly IGameLanguageProvider gameLanguageProvider;

        public PoeTradeClient(
            ILogger<PoeTradeClient> logger,
            IMediator mediator,
            IGameLanguageProvider gameLanguageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.gameLanguageProvider = gameLanguageProvider;
            HttpClient = httpClientFactory.CreateClient();
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
            HttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Sidekick");

            Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };
            Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public JsonSerializerOptions Options { get; }

        public HttpClient HttpClient { get; set; }

        public async Task<FetchResult<TReturn>> Fetch<TReturn>(string path, bool useDefaultLanguage = false)
        {
            var name = typeof(TReturn).Name;

            var language = gameLanguageProvider.Language;

            if (useDefaultLanguage || language == null)
            {
                language = await mediator.Send(new GetGameLanguageQuery("en"));
            }

            try
            {
                var response = await HttpClient.GetAsync(language.PoeTradeApiBaseUrl + path);
                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<FetchResult<TReturn>>(content, Options);
            }
            catch (Exception)
            {
                logger.LogInformation($"Could not fetch {name} at {language.PoeTradeApiBaseUrl + path}.");
                throw;
            }
        }
    }
}
