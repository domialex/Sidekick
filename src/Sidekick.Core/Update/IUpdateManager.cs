using System.Threading.Tasks;

namespace Sidekick.Core.Update
{
    public interface IUpdateManager
    {
        Task<bool> NewVersionAvailable();
        Task<bool> UpdateSidekick();
        void Restart();
    }
}
