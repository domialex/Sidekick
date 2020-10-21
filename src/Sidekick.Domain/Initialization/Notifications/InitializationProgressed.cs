using MediatR;

namespace Sidekick.Domain.Initialization.Notifications
{
    public class InitializationProgressed : INotification
    {
        public string Title { get; set; }
        public int TotalPercentage { get; set; }
    }
}
