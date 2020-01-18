using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializer
    {
        Task Initialize();
        Task Reset();
        bool IsReady { get; }
    }
}
