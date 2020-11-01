using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    /// <summary>
    /// Notification to indicate that the initialization has progressed
    /// </summary>
    public class InitializationProgressed : INotification
    {
        /// <summary>
        /// Notification to indicate that the initialization has progressed
        /// </summary>
        /// <param name="percentage">The current initialization percentage</param>
        public InitializationProgressed(int percentage)
        {
            Percentage = percentage;
        }

        /// <summary>
        /// The title of the current initialization progress
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The current initialization percentage
        /// </summary>
        public int Percentage { get; set; }
    }
}
