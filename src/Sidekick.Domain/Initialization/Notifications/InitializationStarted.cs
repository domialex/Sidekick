using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    /// <summary>
    /// Indicates that an initialization command has started
    /// </summary>
    public class InitializationStarted : INotification
    {
    }
}
