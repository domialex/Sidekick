using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Presentation.ElectronProgram.Notifications
{
    public class OpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        public OpenNotificationHandler()
        {
        }

        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            // Todo: Implement
            return Unit.Task;
        }
    }
}
