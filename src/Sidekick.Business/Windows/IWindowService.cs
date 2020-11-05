using System.Threading.Tasks;
using Sidekick.Persistence.Windows;

namespace Sidekick.Business.Windows
{
    public interface IWindowService
    {
        Task<Window> Get(string id);
        Task SaveSize(string id, double width, double height);
    }
}
