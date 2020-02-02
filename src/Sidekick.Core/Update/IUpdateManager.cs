using System.Threading.Tasks;

namespace Sidekick.Core.Update
{
    public interface IUpdateManager
    {
        string InstallDirectory { get; }
        Task<bool> NewVersionAvailable();
        Task<bool> UpdateSidekick();
    }
}
