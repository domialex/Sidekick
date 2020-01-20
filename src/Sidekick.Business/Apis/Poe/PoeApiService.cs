using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Languages.Client;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public class PoeApiService : IPoeApiService, IOnBeforeInit
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly HttpClient client;

        public PoeApiService(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.client = httpClientFactory.CreateClient();
        }

        public Task OnBeforeInit()
        {
            client.BaseAddress = languageProvider.Language.PoeTradeApiBaseUrl;

            return Task.CompletedTask;
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

        public async Task<List<TReturn>> Fetch<TReturn>()
        {
            string path = string.Empty;
            string name = string.Empty;
            switch (typeof(TReturn).Name)
            {
                case nameof(ItemCategory):
                    name = "items";
                    path += "data/items/";
                    break;
                case nameof(League):
                    name = "leagues";
                    path += "data/leagues/";
                    break;
                case nameof(StaticItemCategory):
                    name = "static items";
                    path += "data/static/";
                    break;
                case nameof(AttributeCategory):
                    name = "attributes";
                    path += "data/stats/";
                    break;
                default: throw new Exception("The type to fetch is not recognized by the PoeApiService.");
            }

            logger.Log($"Fetching {name} started.");
            QueryResult<TReturn> result = null;
            var success = false;

            while (!success)
            {
                try
                {
                    var response = await client.GetAsync(path);
                    var content = await response.Content.ReadAsStreamAsync();

                    result = await JsonSerializer.DeserializeAsync<QueryResult<TReturn>>(content, Options);

                    logger.Log($"{result.Result.Count} {name} fetched.");
                    success = true;
                }
                catch
                {
                    logger.Log($"Could not fetch {name}.");
                    logger.Log("Retrying every minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }

            logger.Log($"Fetching {name} finished.");
            return result.Result;
        }
    }
}
