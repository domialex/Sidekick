using System.Threading.Tasks;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis
{
    public interface IWikiProvider
    {
        Task Open(Item item);
    }
}
