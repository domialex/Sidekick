using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializeService
    {
        Task Initialize();
        Task Reset();
        bool IsReady { get; }
    }
}
