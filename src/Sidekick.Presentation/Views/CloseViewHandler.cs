using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Presentation.Views.Commands
{
    /// <summary>
    /// Close all opened views
    /// </summary>
    public class CloseViewHandler : ICommandHandler<CloseViewCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public CloseViewHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(CloseViewCommand request, CancellationToken cancellationToken)
        {
            var result = false;

            if (viewLocator.IsOpened(View.League))
            {
                viewLocator.Close(View.League);
                result = true;
            }

            if (viewLocator.IsOpened(View.Price))
            {
                viewLocator.Close(View.Price);
                result = true;
            }

            return Task.FromResult(result);
        }
    }
}
