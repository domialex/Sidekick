using System.Threading.Tasks;

namespace Sidekick.Persistence.ItemCategories
{
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> Get(string type);
        Task SaveCategory(string type, string category);
        Task Delete(string type);
    }
}
