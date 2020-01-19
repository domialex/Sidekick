using Newtonsoft.Json;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Http;
using Sidekick.Business.Languages.Client;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public class PoeApiService : IPoeApiService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IHttpClientProvider httpClientProvider;

        public PoeApiService(ILogger logger,
            ILanguageProvider languageProvider,
            IHttpClientProvider httpClientProvider)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.httpClientProvider = httpClientProvider;
        }

        public async Task<List<TReturn>> Fetch<TReturn>(string name, string path)
            where TReturn : class
        {
            logger.Log($"Fetching {name} started.");
            List<TReturn> result = null;
            var success = false;

            while (!success)
            {
                try
                {
                    var response = await httpClientProvider.HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "data/" + path);
                    var content = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<QueryResult<TReturn>>(content, httpClientProvider.JsonSerializerSettings)?.Result;

                    logger.Log($"{result.Count} {name} fetched.");
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
            return result;
        }
    }
}
