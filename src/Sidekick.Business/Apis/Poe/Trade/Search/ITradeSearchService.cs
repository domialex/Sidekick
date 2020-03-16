using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public interface ITradeSearchService
    {
        Task<FetchResult<Result>> GetListings(Parsers.Models.Item item);
        Task<FetchResult<Result>> GetListings(FetchResult<string> queryResult, int page = 0);
        Task<FetchResult<Result>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch);
        Task OpenWebpage(Parsers.Models.Item item);
    }
}
