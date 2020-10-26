using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis
{
    public interface IWikiProvider
    {
        Task Open(Item item);
    }
}
