using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Mock.Notifications
{
    public class MockOpenConfirmNotificationHandler : ICommandHandler<OpenConfirmNotificationCommand>
    {
        public Task<Unit> Handle(OpenConfirmNotificationCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
