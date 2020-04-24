using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
