using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Cheatsheets
{
    public class OpenCheatsheetsHandler : ICommandHandler<OpenCheatsheetsCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly ISidekickSettings settings;

        public OpenCheatsheetsHandler(IViewLocator viewLocator,
            ISidekickSettings settings)
        {
            this.viewLocator = viewLocator;
            this.settings = settings;
        }

        public Task<bool> Handle(OpenCheatsheetsCommand request, CancellationToken cancellationToken)
        {
            viewLocator.Open(View.League, settings.Cheatsheets_Selected);

            return Task.FromResult(true);
        }
    }
}
