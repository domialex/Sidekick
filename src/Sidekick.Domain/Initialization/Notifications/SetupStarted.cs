using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    /// <summary>
    /// Indicates that the initialization command has completed
    /// </summary>
    public class SetupStarted : INotification
    {
    }
}
