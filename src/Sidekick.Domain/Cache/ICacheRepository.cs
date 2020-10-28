using System;
using System.Threading.Tasks;

namespace Sidekick.Domain.Cache
{
    public interface ICacheRepository
    {
        Task<TModel> Get<TModel>(string key)
            where TModel : class;

        Task<TModel> GetOrSet<TModel>(string key, Func<Task<TModel>> func)
            where TModel : class;

        Task Save(string key, object data);

        Task Clear();
    }
}
