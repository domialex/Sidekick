using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IOnInit
    {
        Task OnInit();
    }
}
