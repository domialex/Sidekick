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

        public async Task<List<TReturn>> Fetch<TReturn>(FetchEnum type)
        {
            logger.Log($"Fetching {type.ToString()} started.");
            QueryResult<TReturn> result = null;
            var success = false;

            string path = string.Empty;
            switch (type)
            {
                case FetchEnum.Items: path += "data/items/"; break;
                case FetchEnum.Leagues: path += "data/leagues/"; break;
                case FetchEnum.Static: path += "data/static/"; break;
                case FetchEnum.Stats: path += "data/stats/"; break;
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

                    logger.Log($"{result.Result.Count} {type.ToString()} fetched.");
                    success = true;
                }
                catch
                {
                    logger.Log($"Could not fetch {type.ToString()}.");
                    logger.Log("Retrying every minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }

            logger.Log($"Fetching {type.ToString()} finished.");
            return result.Result;
            return result;
        }
    }
}
