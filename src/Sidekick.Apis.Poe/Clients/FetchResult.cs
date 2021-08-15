using System.Collections.Generic;

namespace Sidekick.Apis.Poe.Clients
{
    public class FetchResult<T>
    {
        public List<T> Result { get; set; }
    }
}
