using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Presentation.Views.Commands
{
    public class CloseMapViewHandler : ICommandHandler<CloseMapViewCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public CloseMapViewHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(CloseMapViewCommand request, CancellationToken cancellationToken)
        {
            var result = viewLocator.IsOpened(View.Map);

            viewLocator.Close(View.Map);

            return Task.FromResult(result);
        }
    }
}
