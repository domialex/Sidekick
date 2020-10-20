using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Initialization.Notifications;
using Sidekick.Views;

namespace Sidekick.Initialization
{
    public class InitializationSettingsNotification : INotificationHandler<InitializeSettingsNotification>
    {
        private readonly IViewLocator viewLocator;

        public InitializationSettingsNotification(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task Handle(InitializeSettingsNotification notification, CancellationToken cancellationToken)
        {
            notification.OnStart("View");
            if (!viewLocator.IsOpened<InitializationView>())
            {
                viewLocator.Open<InitializationView>();
            }
            notification.OnEnd("View");
            return Task.CompletedTask;
        }
    }
}
