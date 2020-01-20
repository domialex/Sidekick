using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IOnReset
    {
        Task OnReset();
    }
}
