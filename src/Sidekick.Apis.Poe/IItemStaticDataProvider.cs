using System.Threading.Tasks;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.Poe
{
    public interface IItemStaticDataProvider
    {
        Task Initialize();

        string GetImage(string id);

        string GetId(string text);

        string GetId(Item item);
    }
}
