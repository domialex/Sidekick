using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Languages;
using Sidekick.Business.Loggers;
using Sidekick.Core.DependencyInjection.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    [SidekickService(typeof(IPoeApiService))]
    public class PoeApiService : IPoeApiService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;

        public PoeApiService(ILogger logger, ILanguageProvider languageProvider)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;

            HttpClient = new HttpClient();

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        private JsonSerializerSettings JsonSerializerSettings { get; set; }

        private HttpClient HttpClient { get; set; }

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
                    var response = await HttpClient.GetAsync(languageProvider.Language.PoeTradeApiBaseUrl + "data/" + path);
                    var content = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<QueryResult<TReturn>>(content, JsonSerializerSettings)?.Result;

                    logger.Log($"{result.Count.ToString().PadRight(3)} {name} fetched.");
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
