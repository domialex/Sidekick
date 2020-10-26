using MediatR;

namespace Sidekick.Domain.Initialization.Queries
{
    /// <summary>
    /// Checks if there is a newer release available on github
    /// </summary>
    /// <returns>true if a new version has been found.</returns>
    public class IsNewVersionAvailableQuery : IQuery<bool>
    {
    }
}
