using System.Net.Http;

namespace Sidekick.Business.Http
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClientProvider()
        {
            HttpClient = new HttpClient();
        }

        public HttpClient HttpClient { get; private set; }
    }
}
