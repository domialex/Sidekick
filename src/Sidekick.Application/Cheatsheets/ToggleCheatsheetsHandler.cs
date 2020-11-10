using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Cheatsheets
{
    public class ToggleCheatsheetsHandler : ICommandHandler<ToggleCheatsheetsCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public ToggleCheatsheetsHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(ToggleCheatsheetsCommand request, CancellationToken cancellationToken)
        {
            if (viewLocator.IsOpened(View.League))
            {
                viewLocator.Close(View.League);
            }
            else
            {
                viewLocator.Open(View.League);
            }

            return Task.FromResult(true);
        }
    }
}
