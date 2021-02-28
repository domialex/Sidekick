using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    /// <summary>
    /// Performs platform actions such as keybinds during the application initialization
    /// </summary>
    public class PlatformInitializationStarted : INotification
    {
    }
}
