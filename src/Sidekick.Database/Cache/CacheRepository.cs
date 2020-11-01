using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Cache;

namespace Sidekick.Database.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private readonly SidekickContext context;

        public CacheRepository(
            SidekickContext context)
        {
            this.context = context;
        }

        public async Task<TModel> GetOrSet<TModel>(string key, Func<Task<TModel>> func)
            where TModel : class
        {
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
            var cache = await context.Caches.FindAsync(key);
            if (cache != null)
            {
                context.Caches.Remove(cache);
                await context.SaveChangesAsync();
            }
        }

        public async Task Clear()
        {
            context.Caches.RemoveRange(await context.Caches.ToListAsync());
            await context.SaveChangesAsync();
        }
    }
}
