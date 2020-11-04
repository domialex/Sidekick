using System;
using System.Threading.Tasks;
using MediatR;

namespace Sidekick.Domain.Notifications.Commands
{
    /// <summary>
    /// Open a notification message with Yes/No buttons
    /// </summary>
    public class OpenConfirmNotificationCommand : ICommand
    {
        /// <summary>
        /// Open a notification message with Yes/No buttons
        /// </summary>
        /// <param name="message">The message to show in the notification</param>
        /// <param name="title">The title of the notification</param>
        /// <param name="onYes">The action to execute when the Yes button is clicked</param>
        /// <param name="onNo">The action to execute when the No button is clicked</param>
        public OpenConfirmNotificationCommand(string message, string title = null, Func<Task> onYes = null, Func<Task> onNo = null)
        {
            Message = message;
            Title = title;
            OnYes = onYes;
            OnNo = onNo;
        }

        /// <summary>
        /// The title of the notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The message to show in the notification
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The action to execute when the Yes button is clicked
        /// </summary>
        public Func<Task> OnYes { get; }

        /// <summary>
        /// The action to execute when the No button is clicked
        /// </summary>
        public Func<Task> OnNo { get; }
    }
}
