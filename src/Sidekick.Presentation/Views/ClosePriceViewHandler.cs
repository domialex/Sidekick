using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Presentation.Views.Commands
{
    public class ClosePriceViewHandler : ICommandHandler<ClosePriceViewCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public ClosePriceViewHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(ClosePriceViewCommand request, CancellationToken cancellationToken)
        {
            var result = viewLocator.IsOpened(View.Price);

            viewLocator.Close(View.Price);

            return Task.FromResult(result);
        }
    }
}
