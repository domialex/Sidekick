using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Presentation.ElectronProgram.Notifications
{
    public class OpenConfirmNotificationHandler : ICommandHandler<OpenConfirmNotificationCommand>
    {
        public OpenConfirmNotificationHandler()
        {
        }

        public Task<Unit> Handle(OpenConfirmNotificationCommand request, CancellationToken cancellationToken)
        {
            // Todo: Implement
            return Unit.Task;
        }
    }
}
