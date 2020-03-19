using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public interface ITradeSearchService
    {
        Task<FetchResult<Result>> GetListings(ParsedItem item, List<StatFilter> stats = null);
        Task<FetchResult<Result>> GetListingsForSubsequentPages(ParsedItem item, int nextPageToFetch, List<StatFilter> stats = null);
        Task OpenWebpage(ParsedItem item);
    }
}
