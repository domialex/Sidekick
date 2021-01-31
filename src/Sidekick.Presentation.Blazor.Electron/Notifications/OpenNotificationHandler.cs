using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Presentation.Blazor.Electron.Notifications
{
    public class OpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            ElectronNET.API.Electron.Notification.Show(new ElectronNET.API.Entities.NotificationOptions(request.Title, request.Message));
            return Unit.Task;
        }
    }
}
