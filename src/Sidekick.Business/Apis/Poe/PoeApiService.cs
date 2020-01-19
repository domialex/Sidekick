using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Languages.Client;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public class PoeApiService : IPoeApiService, IOnBeforeInit
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly Configuration configuration;
        private readonly HttpClient client;

        public PoeApiService(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientFactory httpClientFactory,
            Configuration configuration)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.configuration = configuration;
            this.client = httpClientFactory.CreateClient();
        }

        public Task OnBeforeInit()
        {
            client.BaseAddress = languageProvider.Language.PoeTradeApiBaseUrl;

            return Task.CompletedTask;
        }

        public async Task<List<TReturn>> Fetch<TReturn>()
        {
            var name = typeof(TReturn).Name;

            logger.Log($"Fetching {name} started.");
            QueryResult<TReturn> result = null;
            var success = false;

            string path = string.Empty;
            switch (name)
            {
                case nameof(ItemCategory): path += "data/items/"; break;
                case nameof(League): path += "data/leagues/"; break;
                case nameof(StaticItemCategory): path += "data/static/"; break;
                case nameof(AttributeCategory): path += "data/stats/"; break;
                default: throw new Exception("The type to fetch is not recognized by the PoeApiService.");
            }

            while (!success)
            {
                try
                {
                    var response = await client.GetAsync(path);
                    var content = await response.Content.ReadAsStreamAsync();

                    result = await JsonSerializer.DeserializeAsync<QueryResult<TReturn>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });

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

        public async Task<QueryResult<TReturn>> Query<TReturn>(QueryEnum type, object data)
        {
            logger.Log($"Querying {type.ToString()} started.");
            QueryResult<TReturn> result = null;

            string path = string.Empty;
            switch (type)
            {
                case QueryEnum.Exchange: path += "exchange/"; break;
                case QueryEnum.Search: path += "search/"; break;
            }

            try
            {
                var body = new StringContent(JsonSerializer.Serialize(data));
                var response = await client.PostAsync(path + configuration.LeagueId, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = await JsonSerializer.DeserializeAsync<QueryResult<TReturn>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });

                    switch (type)
                    {
                        case QueryEnum.Exchange:
                            result.Uri = new Uri(languageProvider.Language.PoeTradeExchangeBaseUrl + configuration.LeagueId + "/" + result.Id);
                            break;
                        case QueryEnum.Search:
                            result.Uri = new Uri(languageProvider.Language.PoeTradeSearchBaseUrl + configuration.LeagueId + "/" + result.Id);
                            break;
                    }
                }
            }
            catch
            {
                return null;
            }

            logger.Log($"Querying {type.ToString()} finished.");
            return result;
        }
    }
}
