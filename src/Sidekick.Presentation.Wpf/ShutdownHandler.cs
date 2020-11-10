using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Presentation.Wpf
{
    public class ShutdownHandler : ICommandHandler<ShutdownCommand>
    {
        private readonly App app;

        public ShutdownHandler(App app)
        {
            this.app = app;
        }

        public Task<Unit> Handle(ShutdownCommand request, CancellationToken cancellationToken)
        {
            app.Shutdown();
            return Task.FromResult(Unit.Value);
        }
    }
}
