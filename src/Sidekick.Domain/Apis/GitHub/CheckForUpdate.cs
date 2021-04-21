using MediatR;

namespace Sidekick.Domain.Apis.GitHub
{
    /// <summary>
    /// Checks if there is a newer release available on github.
    /// If there is a new version, download and run the update package.
    /// </summary>
    public class CheckForUpdate : ICommand
    {
    }
}
