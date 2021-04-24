using System;
using System.Threading.Tasks;

namespace Sidekick.Domain.Cache
{
    public interface ICacheRepository
    {
        /// <summary>
        /// Gets the value of the cache if it is set. Otherwise initializes the cache with the Func.
        /// </summary>
        /// <typeparam name="TModel">THe type of the data</typeparam>
        /// <param name="key">The key under which to save the cache</param>
        /// <param name="func">The func to initialize the data, in the event that the data is not in the cache</param>
        /// <returns>Returns the cache data</returns>
        Task<TModel> GetOrSet<TModel>(string key, Func<Task<TModel>> func)
            where TModel : class;

        /// <summary>
        /// Clears the cache
        /// </summary>
        /// <returns></returns>
        Task Clear();
    }
}
