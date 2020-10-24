using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Presentation.Views;

namespace Sidekick.Presentation.Initialization.Notifications
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
            viewLocator.Close(View.Settings);
            viewLocator.Close(View.Setup);
            if (settings.ShowSplashScreen && !viewLocator.IsOpened(View.Initialization))
            {
                viewLocator.Open(View.Initialization);
            }
            return Task.CompletedTask;
        }
    }
}
