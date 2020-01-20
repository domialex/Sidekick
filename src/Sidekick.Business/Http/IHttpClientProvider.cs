using System.Net.Http;

namespace Sidekick.Business.Http
{
    public interface IHttpClientProvider
    {
        HttpClient HttpClient { get; }
    }
}
