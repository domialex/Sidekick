using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Languages;

namespace Sidekick.Business.Apis.Poe
{
    public class PoeApiClient : IPoeApiClient
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly HttpClient client;

        public PoeApiClient(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            client = httpClientFactory.CreateClient();
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
            string name;
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

            try
            {
                logger.LogInformation($"Fetching {name} started.");

                var response = await client.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + path);
                var content = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FetchResult<TReturn>>(content, Options);

                logger.LogInformation($"{result.Result.Count} {name} fetched.");
                return result.Result;
            }
            catch (Exception)
            {
                logger.LogInformation($"Could not fetch {name}.");
                throw;
            }

        }
    }
}
