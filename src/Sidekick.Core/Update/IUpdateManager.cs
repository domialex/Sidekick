using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Update
{
    public interface IUpdateManager
    {
        Action<string, int> ReportProgress { set; }
        Task<bool> NewVersionAvailable();
        Task<bool> UpdateSidekick();
        void Restart();
    }
}
