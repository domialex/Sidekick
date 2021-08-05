using System.Threading.Tasks;

namespace Sidekick.Apis.GitHub
{
    public interface IGitHubClient
    {
        Task<bool> Update();
    }
}
