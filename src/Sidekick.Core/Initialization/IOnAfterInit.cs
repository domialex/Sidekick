using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IOnAfterInit
    {
        Task OnAfterInit();
    }
}
