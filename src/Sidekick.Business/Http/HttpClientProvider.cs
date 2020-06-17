using System.Net.Http;

namespace Sidekick.Business.Http
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public HttpClient HttpClient
        {
            get
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-Powered-By", "Sidekick");
                return client;
            }
        }
    }
}
