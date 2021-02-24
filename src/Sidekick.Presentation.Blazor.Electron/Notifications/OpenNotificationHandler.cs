using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Presentation.Blazor.Electron.Notifications
{
    public class OpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        private IWebHostEnvironment WebHostEnvironment;

        public OpenNotificationHandler(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationOptions = new ElectronNET.API.Entities.NotificationOptions(request.Title, request.Message)
            {
                Icon = $"{WebHostEnvironment.ContentRootPath}Assets/icon.png"
            };

            ElectronNET.API.Electron.Notification.Show(notificationOptions);
            return Unit.Task;
        }
    }
}
