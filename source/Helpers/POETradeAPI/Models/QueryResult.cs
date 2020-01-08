using Sidekick.Helpers.POENinjaAPI.Models;
using System;
using System.Collections.Generic;

namespace Sidekick.Helpers.POETradeAPI.Models
{
    public class QueryResult<T>
    {
        public List<T> Result { get; set; }
        public string Id { get; set; }
        public int Total { get; set; }
        public Item Item { get; set; }
        public string Uri { get; set; }

        public PoeNinjaItem PoeNinjaItem { get; set; }

        public DateTime? LastRefreshTimestamp { get; set; }
    }
}
