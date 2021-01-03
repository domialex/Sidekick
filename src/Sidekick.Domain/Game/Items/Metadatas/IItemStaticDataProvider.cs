using System.Threading.Tasks;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Items.Metadatas
{
    public interface IItemStaticDataProvider
    {
        Task Initialize();
        string GetImage(string id);
        string GetId(string text);
        string GetId(Item item);
    }
}
