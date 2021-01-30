using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Presentation.Blazor.Electron.App
{
    public class ShutdownHandler : ICommandHandler<ShutdownCommand>
    {
        public Task<Unit> Handle(ShutdownCommand request, CancellationToken cancellationToken)
        {
            ElectronNET.API.Electron.App.Exit();
            return Unit.Task;
        }
    }
}
