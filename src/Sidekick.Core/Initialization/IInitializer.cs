using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializer
    {
        Task Initialize();
        event Action<ErrorEventArgs> OnError;
        event Action<ProgressEventArgs> OnProgress;
        void ReportProgress(InitializationSteps progressType, string serviceName, string message);
    }
}
