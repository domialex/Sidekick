using System.Threading.Tasks;

namespace Sidekick.Domain.Views
{
    public interface IViewPreferenceRepository
    {
        Task<ViewPreference> Get(View id);
        Task SaveSize(View id, int width, int height);
    }
}
