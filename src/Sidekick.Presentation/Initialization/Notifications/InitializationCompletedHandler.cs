using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using Sidekick.Localization.Tray;

namespace Sidekick.Presentation.Initialization.Notifications
{
    public class InitializationCompletedHandler : INotificationHandler<InitializationCompleted>
    {
        private readonly ISidekickSettings settings;
        private readonly IMediator mediator;
        private readonly IViewLocator viewLocator;

        public InitializationCompletedHandler(
            ISidekickSettings settings,
            IMediator mediator,
            IViewLocator viewLocator)
        {
            this.settings = settings;
            this.mediator = mediator;
            this.viewLocator = viewLocator;
        }

        public async Task Handle(InitializationCompleted notification, CancellationToken cancellationToken)
        {
            // If we have a successful initialization, we delay for half a second to show the "Ready" label on the UI before closing the view
            await Task.Delay(500);

            // Show a system notification
            await mediator.Send(new OpenNotificationCommand(string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString()), true)
            {
                Title = TrayResources.Notification_Title,
            });

            viewLocator.Close(View.Initialization);
        }
    }
}
