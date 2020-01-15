using System.Net.Http;

namespace Sidekick.Helpers
{
    public static class HttpClientProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static HttpClient GetHttpClient()
            => _httpClient;
    }
}
