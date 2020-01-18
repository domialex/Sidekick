using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IOnBeforeInit
    {
        Task OnBeforeInit();
    }
}
