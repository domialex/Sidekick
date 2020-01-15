using Sidekick.Business.Parsers.Models;
using System;
using System.Collections.Generic;

namespace Sidekick.Business.Trades.Results
{
    public class QueryResult<T>
    {
        public List<T> Result { get; set; }
        public string Id { get; set; }
        public int Total { get; set; }
        public Item Item { get; set; }
        public Uri Uri { get; set; }
    }
}
