using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sidekick.Core.DependencyInjection.Services;
using System;
using System.Net.Http;

namespace Sidekick.Business.Http
{
    [Obsolete]
    [SidekickService(typeof(IHttpClientProvider))]
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClientProvider()
        {
            HttpClient = new HttpClient();

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public JsonSerializerSettings JsonSerializerSettings { get; private set; }

        public HttpClient HttpClient { get; private set; }
    }
}
