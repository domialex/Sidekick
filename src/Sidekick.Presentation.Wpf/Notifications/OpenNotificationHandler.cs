using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using MediatR;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Presentation.Wpf;

namespace Sidekick.Notifications
{
    public class OpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        private readonly Dispatcher dispatcher;
        private readonly App app;

        public OpenNotificationHandler(Dispatcher dispatcher, App app)
        {
            this.dispatcher = dispatcher;
            this.app = app;
        }

        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            if (request.IsSystemNotification)
            {
                dispatcher.Invoke(() =>
                {
                    app.TrayIcon.ShowBalloonTip(
                        request.Title ?? "Sidekick",
                        request.Message,
                        app.TrayIcon.Icon,
                        largeIcon: true);
                });
            }
            else
            {
                dispatcher.Invoke(() =>
                {
                    AdonisUI.Controls.MessageBox.Show(request.Message, request.Title, buttons: AdonisUI.Controls.MessageBoxButton.OK);
                });
            }

            return Unit.Task;
        }
    }
}
