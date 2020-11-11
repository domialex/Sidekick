using System;
using System.Threading.Tasks;
using Sidekick.Domain.Cache;

namespace Sidekick.TestInfrastructure.TestClients.Caches
{
    public class TestCacheRepository : ICacheRepository
    {
        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetOrSet<TModel>(string key, Func<Task<TModel>> func) where TModel : class
        {
            return func();
        }
    }
}
