using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializer
    {
        Task Initialize();
        Task Reset();
        bool IsReady { get; }
        event Action<ProgressEventArgs> OnProgress;
        void ReportProgress(ProgressTypeEnum progressType, string serviceName, string message);
    }
}
