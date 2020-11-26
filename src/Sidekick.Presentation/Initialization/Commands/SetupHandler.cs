using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Initialization.Commands
{
    public class SetupHandler : ICommandHandler<SetupCommand>
    {
        private readonly IViewLocator viewLocator;

        public SetupHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<Unit> Handle(SetupCommand request, CancellationToken cancellationToken)
        {
            viewLocator.Close(View.Initialization);
            viewLocator.Open(View.Setup);
            return Unit.Task;
        }
    }
}
