using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Database;
using Sidekick.Database.Caches;

namespace Sidekick.Business.Caches
{
    public class CacheService : ICacheService
    {
        private readonly DbContextOptions<SidekickContext> options;

        public CacheService(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<TModel> Get<TModel>(string key)
            where TModel : class
        {
            using var dbContext = new SidekickContext(options);

            var data = await dbContext.Caches.Where(x => x.Key == key).Select(x => x.Data).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<TModel>(data);
            }
            catch (Exception)
            {
                await Delete(key);
                return default;
            }
        }

        public async Task<TModel> GetOrCreate<TModel>(string key, Func<Task<TModel>> create)
            where TModel : class
        {
            var result = await Get<TModel>(key);

            if (result == default)
            {
                result = await create.Invoke();
                await Save(key, result);
            }

            return result;
        }

        public async Task Save<TModel>(string key, TModel data)
            where TModel : class
        {
            using var dbContext = new SidekickContext(options);

            var cache = await dbContext.Caches.Where(x => x.Key == key).FirstOrDefaultAsync();

            if (cache == null)
            {
                cache = new Cache()
                {
                    Key = key,
                };
                dbContext.Caches.Add(cache);
            }

            cache.Data = JsonSerializer.Serialize(data);

            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(string key)
        {
            using var dbContext = new SidekickContext(options);

            var cache = await dbContext.Caches.Where(x => x.Key == key).FirstOrDefaultAsync();
            if (cache != null)
            {
                dbContext.Caches.Remove(cache);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Clear()
        {
            using var dbContext = new SidekickContext(options);
            dbContext.Caches.RemoveRange(await dbContext.Caches.ToListAsync());
            await dbContext.SaveChangesAsync();
        }
    }
}
