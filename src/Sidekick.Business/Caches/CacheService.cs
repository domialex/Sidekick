using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Database;
using Sidekick.Database.Caches;

namespace Sidekick.Business.Caches
{
    public class CacheService : ICacheService
    {
        private readonly DbContextOptions<SidekickContext> options;
        private readonly ILogger logger;

        public CacheService(DbContextOptions<SidekickContext> options,
            ILogger<CacheService> logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public async Task<TModel> Get<TModel>(string key)
            where TModel : class
        {
            logger.LogDebug($"CacheService : Getting data for {key}");

            using var dbContext = new SidekickContext(options);

            var cache = await dbContext.Caches.FindAsync(key);

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
            logger.LogDebug($"CacheService : Saving data for {key}");

            using var dbContext = new SidekickContext(options);

            var cache = await dbContext.Caches.FindAsync(key);

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
            logger.LogDebug($"CacheService : Deleting data for {key}");

            using var dbContext = new SidekickContext(options);

            var cache = await dbContext.Caches.FindAsync(key);
            if (cache != null)
            {
                dbContext.Caches.Remove(cache);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Clear()
        {
            logger.LogDebug($"CacheService : Clearing data");

            using var dbContext = new SidekickContext(options);
            dbContext.Caches.RemoveRange(await dbContext.Caches.ToListAsync());
            await dbContext.SaveChangesAsync();
        }
    }
}
