using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Mock.App
{
    public class MockShutdownHandler : ICommandHandler<ShutdownCommand>
    {
        public Task<Unit> Handle(ShutdownCommand request, CancellationToken cancellationToken)
        {
            Environment.Exit(Environment.ExitCode);
            return Unit.Task;
        }
    }
}
