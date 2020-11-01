using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sidekick.Persistence;
using Sidekick.Persistence.ItemCategories;

namespace Sidekick.Business.ItemCategories
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly DbContextOptions<SidekickContext> options;
        private readonly ILogger logger;

        public ItemCategoryService(DbContextOptions<SidekickContext> options,
            ILogger<ItemCategoryService> logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public async Task<ItemCategory> Get(string type)
        {
            logger.LogDebug($"ItemCategoryService : Getting data for {type}");

            using var dbContext = new SidekickContext(options);

            return await dbContext.ItemCategories.FindAsync(type);
        }

        public async Task SaveCategory(string type, string category)
        {
            logger.LogDebug($"ItemCategoryService : Saving data for {type}");

            using var dbContext = new SidekickContext(options);

            var itemCategory = await dbContext.ItemCategories.FindAsync(type);

            if (itemCategory == null)
            {
                itemCategory = new ItemCategory()
                {
                    Type = type,
                };
                dbContext.ItemCategories.Add(itemCategory);
            }

            itemCategory.Category = category;

            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(string type)
        {
            logger.LogDebug($"ItemCategoryService : Deleting data for {type}");

            using var dbContext = new SidekickContext(options);

            var itemCategory = await dbContext.ItemCategories.FindAsync(type);

            if (itemCategory != null)
            {
                dbContext.ItemCategories.Remove(itemCategory);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
