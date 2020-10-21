using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    public class InitializationProgressed : INotification
    {
        public InitializationProgressed(int percentage)
        {
            Percentage = percentage;
        }

        public string Title { get; set; }
        public int Percentage { get; set; }
    }
}
