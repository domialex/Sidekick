using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Requests
{
    public class QueryRequest
    {
        public Query Query { get; set; } = new Query();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
