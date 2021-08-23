using System.Threading.Tasks;
using Sidekick.Apis.GitHub.Models;

namespace Sidekick.Apis.GitHub
{
    public interface IGitHubClient
    {
        Task<GitHubRelease> GetLatestRelease();
        bool IsUpdateAvailable(GitHubRelease release);
        Task<string> DownloadRelease(GitHubRelease release);
    }
}
