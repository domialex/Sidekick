using MediatR;

namespace Sidekick.Domain.Notifications.Commands
{
    /// <summary>
    /// Open a notification message
    /// </summary>
    public class OpenNotificationCommand : ICommand
    {
        /// <summary>
        /// Open a notification message
        /// </summary>
        /// <param name="message">The message to show in the notification</param>
        /// <param name="isSystemNotification">If true, the notification will show as a system (Windows) tooltip; if false, as an application window.</param>
        public OpenNotificationCommand(string message, bool isSystemNotification = false)
        {
            Message = message;
            IsSystemNotification = isSystemNotification;
        }

        /// <summary>
        /// The title of the notification (optional)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The message to show in the notification
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// If true, the notification will show as a system (Windows) tooltip; if false, as an application window.
        /// </summary>
        public bool IsSystemNotification { get; set; }
    }
}
