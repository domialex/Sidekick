using Sidekick.Core.DependencyInjection.Services;
using System.Net.Http;

namespace Sidekick.Business.Http
{
    [SidekickService(typeof(IHttpClientProvider))]
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClientProvider()
        {
            HttpClient = new HttpClient();
        }

        public HttpClient HttpClient { get; private set; }
    }
}
