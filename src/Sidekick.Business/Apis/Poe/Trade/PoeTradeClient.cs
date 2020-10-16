using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Leagues;
using Sidekick.Business.Languages;

namespace Sidekick.Business.Apis.Poe.Trade
{
    public class PoeTradeClient : IPoeTradeClient
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly HttpClient client;

        public PoeTradeClient(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger.ForContext(GetType());
            this.languageProvider = languageProvider;
            client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("Sidekick");
        }

        public JsonSerializerOptions Options
        {
            get
            {
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true,
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                return options;
            }
        }

        public async Task<List<TReturn>> Fetch<TReturn>(bool useEnglishLanguage = false)
        {
            string path;
            string name;
            switch (typeof(TReturn).Name)
            {
                case nameof(ItemDataCategory):
                    name = "items";
                    path = "data/items";
                    break;
                case nameof(League):
                    name = "leagues";
                    path = "data/leagues";
                    break;
                case nameof(StaticItemCategory):
                    name = "static items";
                    path = "data/static";
                    break;
                case nameof(StatDataCategory):
                    name = "attributes";
                    path = "data/stats";
                    break;
                default: throw new Exception("The type to fetch is not recognized by the PoeApiService.");
            }

            var language = useEnglishLanguage ? languageProvider.EnglishLanguage : languageProvider.Language;

            try
            {
                logger.Information($"Fetching {name} started.");

                var response = await client.GetAsync(language.PoeTradeApiBaseUrl + path);
                var content = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FetchResult<TReturn>>(content, Options);

                logger.Information($"{result.Result.Count} {name} fetched.");
                return result.Result;
            }
            catch (Exception)
            {
                logger.Information($"Could not fetch {name} at {language.PoeTradeApiBaseUrl + path}.");
                throw;
            }

        }
    }
}
