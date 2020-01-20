using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Trades.Results;
using System.Threading.Tasks;

namespace Sidekick.Business.Trades
{
    public interface ITradeClient
    {
        Task<QueryResult<ListingResult>> GetListings(Parsers.Models.Item item);
        Task<QueryResult<ListingResult>> GetListings(QueryResult<string> queryResult, int page = 0);
        Task<QueryResult<ListingResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch);
        Task<QueryResult<string>> Query(Parsers.Models.Item item);
        Task OpenWebpage(Parsers.Models.Item item);
    }
}
