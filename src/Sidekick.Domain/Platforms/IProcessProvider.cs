using System.Threading;
using System.Threading.Tasks;

namespace Sidekick.Domain.Platforms
{
    public interface IProcessProvider
    {
        Task Initialize(CancellationToken cancellationToken);
        string ClientLogPath { get; }
        bool IsPathOfExileInFocus();
        bool IsSidekickInFocus();
    }
}
