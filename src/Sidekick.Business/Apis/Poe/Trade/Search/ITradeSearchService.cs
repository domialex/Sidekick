using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public interface ITradeSearchService
    {
        Task<FetchResult<Result>> GetListings(ParsedItem item);
        Task<FetchResult<Result>> GetListings(FetchResult<string> queryResult, int page = 0);
        Task<FetchResult<Result>> GetListingsForSubsequentPages(ParsedItem item, int nextPageToFetch);
        Task OpenWebpage(ParsedItem item);
    }
}
