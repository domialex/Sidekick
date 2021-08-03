using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sidekick.Common.Platform
{
    public interface IProcessProvider
    {
        Task Initialize(CancellationToken cancellationToken);

        string ClientLogPath { get; }

        event Action OnFocus;
        event Action OnBlur;

        bool IsPathOfExileInFocus { get; }
        bool IsSidekickInFocus { get; }
    }
}
