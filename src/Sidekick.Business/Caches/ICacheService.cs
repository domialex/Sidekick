using System;
using System.Threading.Tasks;

namespace Sidekick.Business.Caches
{
    public interface ICacheService
    {
        Task<TModel> Get<TModel>(string key)
            where TModel : class;

        Task<TModel> GetOrCreate<TModel>(string key, Func<Task<TModel>> create)
            where TModel : class;

        Task Save<TModel>(string key, TModel data)
            where TModel : class;

        Task Clear();
    }
}
