using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public interface IPoeApiService
    {
        Task<List<TReturn>> Fetch<TReturn>();

        JsonSerializerOptions Options { get; }
    }
}
