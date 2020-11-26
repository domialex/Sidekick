using System.Net.Http;
using System.Text.Json;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public interface IPoePriceInfoClient
    {
        JsonSerializerOptions Options { get; }

        HttpClient Client { get; }
    }
}
