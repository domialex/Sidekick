using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeApi
{
    public class FetchResult<T>
    {
        public List<T> Result { get; set; }
    }
}
