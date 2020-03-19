using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe.Trade
{
    public interface IPoeTradeClient
    {
        Task<List<TReturn>> Fetch<TReturn>();

        JsonSerializerOptions Options { get; }
    }
}
