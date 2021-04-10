using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Mock.Notifications
{
    public class MockOpenNotificationHandler : ICommandHandler<OpenNotificationCommand>
    {
        public Task<Unit> Handle(OpenNotificationCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
