using Sidekick.Business.Apis.Poe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public interface IPoeApiService
    {
        Task<List<TReturn>> Fetch<TReturn>();

        Task<QueryResult<TReturn>> Query<TReturn>(QueryEnum type, object data);
    }
}
