using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe
{
    public interface IPoeApiService
    {
        Task<List<TReturn>> Fetch<TReturn>(string name, string path)
            where TReturn : class;
    }
}
