using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Presentation.Views.Commands
{
    public class OpenViewHandler : ICommandHandler<OpenViewCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public OpenViewHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(OpenViewCommand request, CancellationToken cancellationToken)
        {
            viewLocator.Close(request.View);
            viewLocator.Open(request.View);
            return Task.FromResult(true);
        }
    }
}
