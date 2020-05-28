using System.Net.Http;

namespace Sidekick.TestInfrastructure
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private static readonly HttpClient client = new HttpClient();

        public HttpClient CreateClient(string name)
        {
            return client;
        }
    }
}
