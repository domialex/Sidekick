using System.Threading.Tasks;

namespace Sidekick.Core.Update
{
    public interface IUpdater
    {
        Task Update();
    }
}
