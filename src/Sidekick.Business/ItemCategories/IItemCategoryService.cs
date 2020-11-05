using System.Threading.Tasks;
using Sidekick.Persistence.ItemCategories;

namespace Sidekick.Business.ItemCategories
{
    public interface IItemCategoryService
    {
        Task<ItemCategory> Get(string type);
        Task SaveCategory(string type, string category);
        Task Delete(string type);
    }
}
