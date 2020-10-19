using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializer
    {
        Task Initialize(bool firstRun);
        event Action OnError;
        event Action<ProgressEventArgs> OnProgress;
    }
}
