using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Trades.Results;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    public interface ITradeClient
    {
        Task<QueryResult<SearchResult>> GetListings(Parsers.Models.Item item);
        Task<QueryResult<SearchResult>> GetListings(QueryResult<string> queryResult, int page = 0);
        Task<QueryResult<SearchResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch);
        Task OpenWebpage(Parsers.Models.Item item);
    }
}
