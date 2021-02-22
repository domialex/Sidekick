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
        /// <param name="title">The title of the notification (optional)</param>
        public OpenNotificationCommand(string message, string title = null)
        {
            Message = message;
            Title = title;
        }

        /// <summary>
        /// The title of the notification (optional)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The message to show in the notification
        /// </summary>
        public string Message { get; set; }
    }
}
