using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Trades.Results;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    public interface ITradeClient
    {
        Task<FetchResult<SearchResult>> GetListings(Parsers.Models.Item item);
        Task<FetchResult<SearchResult>> GetListings(FetchResult<string> queryResult, int page = 0);
        Task<FetchResult<SearchResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch);
        Task OpenWebpage(Parsers.Models.Item item);
    }
}
