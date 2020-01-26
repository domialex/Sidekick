using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Update
{
    public interface IUpdateManager
    {
        string InstallDirectory { get; }
        string TempDirectory { get; }
        string ZipPath { get; }
        Action<string, int> ReportProgress { set; }
        Task<bool> NewVersionAvailable();
        Task<bool> UpdateSidekick();
    }
}
