using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public interface IPoeApiClient
    {
        Task<List<TReturn>> Fetch<TReturn>();

        JsonSerializerOptions Options { get; }
    }
}
