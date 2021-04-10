using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Application.Views
{
    public class CloseOverlayHandler : ICommandHandler<CloseOverlayCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public CloseOverlayHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(CloseOverlayCommand request, CancellationToken cancellationToken)
        {
            var result = viewLocator.IsOpened(View.Map) || viewLocator.IsOpened(View.Trade);

            viewLocator.Close(View.Map);
            viewLocator.Close(View.Trade);

            return Task.FromResult(result);
        }
    }
}
