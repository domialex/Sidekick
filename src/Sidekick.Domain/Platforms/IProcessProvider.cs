using System.Threading;
using System.Threading.Tasks;

namespace Sidekick.Domain.Platforms
{
    public interface IProcessProvider
    {
        Task Initialize(CancellationToken cancellationToken);
        bool IsPathOfExileInFocus { get; }
        bool IsSidekickInFocus { get; }
        string ClientLogPath { get; }
    }
}
