using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Natives.App;
using Sidekick.Domain.Natives.Initialization.Commands;

namespace Sidekick.Application.Natives.App
{
    public class ShutdownHandler : ICommandHandler<ShutdownCommand>
    {
        private readonly INativeApp nativeApp;

        public ShutdownHandler(INativeApp nativeApp)
        {
            this.nativeApp = nativeApp;
        }

        public Task<Unit> Handle(ShutdownCommand request, CancellationToken cancellationToken)
        {
            nativeApp.Shutdown();
            return Task.FromResult(Unit.Value);
        }
    }
}
