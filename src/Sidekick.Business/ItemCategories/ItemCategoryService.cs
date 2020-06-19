using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Database;
using Sidekick.Database.ItemCategories;

namespace Sidekick.Business.ItemCategories
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly DbContextOptions<SidekickContext> options;

        public ItemCategoryService(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<ItemCategory> Get(string type)
        {
            using var dbContext = new SidekickContext(options);

            return await dbContext.ItemCategories.Where(x => x.Type == type).FirstOrDefaultAsync();
        }

        public async Task SaveCategory(string type, string category)
        {
            using var dbContext = new SidekickContext(options);

            var itemCategory = await dbContext.ItemCategories.Where(x => x.Type == type).FirstOrDefaultAsync();

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
            using var dbContext = new SidekickContext(options);

            var itemCategory = await dbContext.ItemCategories.Where(x => x.Type == type).FirstOrDefaultAsync();

            if (itemCategory != null)
            {
                dbContext.ItemCategories.Remove(itemCategory);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
