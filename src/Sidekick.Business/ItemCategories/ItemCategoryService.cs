using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidekick.Database;
using Sidekick.Database.ItemCategories;

namespace Sidekick.Business.ItemCategories
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly DbContextOptions<SidekickContext> options;
        private readonly ILogger logger;

        public ItemCategoryService(DbContextOptions<SidekickContext> options,
            ILogger logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public async Task<ItemCategory> Get(string type)
        {
            logger.Debug($"ItemCategoryService : Getting data for {type}");

            using var dbContext = new SidekickContext(options);

            return await dbContext.ItemCategories.FindAsync(type);
        }

        public async Task SaveCategory(string type, string category)
        {
            logger.Debug($"ItemCategoryService : Saving data for {type}");

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
            logger.Debug($"ItemCategoryService : Deleting data for {type}");

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
