using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Presentation.Blazor.Mocks
{
    public class OpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
