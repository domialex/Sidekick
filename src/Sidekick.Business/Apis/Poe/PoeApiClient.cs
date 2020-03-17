using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe
{
    public class PoeApiClient : IPoeApiClient, IOnBeforeInit
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly HttpClient client;

        public PoeApiClient(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger.ForContext(GetType());
            this.languageProvider = languageProvider;
            this.client = httpClientFactory.CreateClient();
        }

        public Task OnBeforeInit()
        {
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
            string path;
            string name = string.Empty;
            switch (typeof(TReturn).Name)
            {
                case nameof(ItemCategory):
                    name = "items";
                    path = "data/items/";
                    break;
                case nameof(League):
                    name = "leagues";
                    path = "data/leagues/";
                    break;
                case nameof(StaticItemCategory):
                    name = "static items";
                    path = "data/static/";
                    break;
                case nameof(AttributeCategory):
                    name = "attributes";
                    path = "data/stats/";
                    break;
                default: throw new Exception("The type to fetch is not recognized by the PoeApiService.");
            }

            logger.Information("Fetching {apiCategory} started.", name);
            QueryResult<TReturn> result = null;
            var success = false;
            var retryAttempts = 1;

            while (!success)
            {
                try
                {
                    var response = await client.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + path);
                    var content = await response.Content.ReadAsStreamAsync();

                    result = await JsonSerializer.DeserializeAsync<QueryResult<TReturn>>(content, Options);

                    logger.Information($"{result.Result.Count} {name} fetched.");
                    success = true;
                }
                catch (Exception ex)
                {
                    logger.Information($"Could not fetch {name}.");

                    retryAttempts--;
                    if (retryAttempts <= 0)
                    {
                        throw;
                    }
                    else
                    {
                        logger.Information("Retrying in 10 seconds.");
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
            }

            logger.Information("Fetching {apiCategory} finished.", name);
            return result.Result;
        }
    }
}
