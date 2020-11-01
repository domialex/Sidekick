using System.Net.Http;

namespace Sidekick.Infrastructure.Github
{
    public interface IGithubClient
    {
        HttpClient Client { get; }
    }
}
