using System;
using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class QueryResult<T>
    {
        public List<T> Result { get; set; }

        public string Id { get; set; }

        public int Total { get; set; }

        public Uri Uri { get; set; }
    }
}
