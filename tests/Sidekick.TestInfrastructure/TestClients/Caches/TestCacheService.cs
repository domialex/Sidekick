using System;
using System.Threading.Tasks;
using Sidekick.Business.Caches;

namespace Sidekick.TestInfrastructure.TestClients.Caches
{
    public class TestCacheService : ICacheService
    {
        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public Task Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> Get<TModel>(string key) where TModel : class
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> GetOrCreate<TModel>(string key, Func<Task<TModel>> create) where TModel : class
        {
            return await create();
        }

        public Task Save<TModel>(string key, TModel data) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}
