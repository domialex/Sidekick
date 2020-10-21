using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Views;

namespace Sidekick.Initialization
{
    public class InitializationStartedHandler : INotificationHandler<InitializationStarted>
    {
        private readonly IViewLocator viewLocator;
        private readonly SidekickSettings settings;

        public InitializationStartedHandler(
            IViewLocator viewLocator,
            SidekickSettings settings)
        {
            this.viewLocator = viewLocator;
            this.settings = settings;
        }

        public Task Handle(InitializationStarted notification, CancellationToken cancellationToken)
        {
            if (settings.ShowSplashScreen && !viewLocator.IsOpened<InitializationView>())
            {
                viewLocator.Open<InitializationView>();
            }
            return Task.CompletedTask;
        }
    }
}
