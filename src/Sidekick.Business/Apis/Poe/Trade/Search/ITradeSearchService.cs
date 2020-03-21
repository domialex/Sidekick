using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public interface ITradeSearchService
    {
        Task<FetchResult<string>> SearchBulk(ParsedItem item);
        Task<FetchResult<string>> Search(ParsedItem item, SearchFilters filters = null, List<StatFilter> stats = null);
        Task<FetchResult<Result>> GetResults(string queryId, List<string> ids);
        Task OpenWebpage(ParsedItem item);
    }
}
