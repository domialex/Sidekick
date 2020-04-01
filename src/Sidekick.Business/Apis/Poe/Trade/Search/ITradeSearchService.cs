using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Business.Apis.Poe.Trade.Search
{
    public interface ITradeSearchService
    {
        Task<FetchResult<string>> SearchBulk(Item item);
        Task<FetchResult<string>> Search(Item item, SearchFilters filters = null, List<StatFilter> stats = null);
        Task<FetchResult<TradeItem>> GetResults(string queryId, List<string> ids, List<StatFilter> stats = null);
        Task OpenWebpage(Item item);
    }
}
