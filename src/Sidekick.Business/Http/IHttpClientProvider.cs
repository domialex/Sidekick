using Newtonsoft.Json;
using System.Net.Http;

namespace Sidekick.Business.Http
{
    public interface IHttpClientProvider
    {
        HttpClient HttpClient { get; }

        JsonSerializerSettings JsonSerializerSettings { get; }
    }
}
