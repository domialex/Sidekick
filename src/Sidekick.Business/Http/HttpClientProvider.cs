using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Http;

namespace Sidekick.Business.Http
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        public JsonSerializerSettings JsonSerializerSettings { get; private set; }

        public HttpClient HttpClient => httpClientFactory.CreateClient();
    }
}
