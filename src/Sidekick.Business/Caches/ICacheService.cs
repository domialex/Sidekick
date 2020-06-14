using System.Threading.Tasks;

namespace Sidekick.Business.Caches
{
    public interface ICacheService
    {
        Task<TModel> Get<TModel>(string key);
        Task Save<TModel>(string key, TModel data);
        Task Clear();
    }
}
