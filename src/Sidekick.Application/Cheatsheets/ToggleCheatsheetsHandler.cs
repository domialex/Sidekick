using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Cheatsheets
{
    public class ToggleCheatsheetsHandler : ICommandHandler<ToggleCheatsheetsCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly ISidekickSettings settings;

        public ToggleCheatsheetsHandler(IViewLocator viewLocator,
            ISidekickSettings settings)
        {
            this.viewLocator = viewLocator;
            this.settings = settings;
        }

        public Task<bool> Handle(ToggleCheatsheetsCommand request, CancellationToken cancellationToken)
        {
            if (viewLocator.IsOpened(View.League))
            {
                viewLocator.Close(View.League);
            }
            else
            {
                viewLocator.Open(View.League, settings.Cheatsheets_Selected);
            }

            return Task.FromResult(true);
        }
    }
}
