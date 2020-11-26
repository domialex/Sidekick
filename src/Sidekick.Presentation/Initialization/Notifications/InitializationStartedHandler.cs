using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Initialization.Notifications
{
    public class InitializationStartedHandler : INotificationHandler<InitializationStarted>
    {
        private readonly IViewLocator viewLocator;
        private readonly ISidekickSettings settings;

        public InitializationStartedHandler(
            IViewLocator viewLocator,
            ISidekickSettings settings)
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
