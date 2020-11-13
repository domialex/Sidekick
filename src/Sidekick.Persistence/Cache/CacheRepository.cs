using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Cache;

namespace Sidekick.Persistence.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private readonly DbContextOptions<SidekickContext> options;

        public CacheRepository(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<TModel> GetOrSet<TModel>(string key, Func<Task<TModel>> func)
            where TModel : class
        {
            using var context = new SidekickContext(options);
            var result = await Get<TModel>(key);

            if (result == default)
            {
                result = await func.Invoke();
                await Save(key, result);
            }

            return result;
        }

        private async Task<TModel> Get<TModel>(string key)
            where TModel : class
        {
            using var context = new SidekickContext(options);
            var cache = await context.Caches.FindAsync(key);

            if (string.IsNullOrEmpty(cache?.Data))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<TModel>(cache.Data);
            }
            catch (Exception)
            {
                await Delete(key);
                return default;
            }
        }

        private async Task Save(string key, object data)
        {
            using var context = new SidekickContext(options);
            var cache = await context.Caches.FindAsync(key);

            if (cache == null)
            {
                cache = new Cache()
                {
                    Key = key,
                };
                context.Caches.Add(cache);
            }

            cache.Data = JsonSerializer.Serialize(data);

            await context.SaveChangesAsync();
        }

        public async Task Delete(string key)
        {
            using var context = new SidekickContext(options);
            var cache = await context.Caches.FindAsync(key);
            if (cache != null)
            {
                context.Caches.Remove(cache);
                await context.SaveChangesAsync();
            }
        }

        public async Task Clear()
        {
            using var context = new SidekickContext(options);
            context.Caches.RemoveRange(await context.Caches.ToListAsync());
            await context.SaveChangesAsync();
        }
    }
}
