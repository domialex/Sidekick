using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Settings
{
    public class OpenSettingsHandler : ICommandHandler<OpenSettingsCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public OpenSettingsHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(OpenSettingsCommand request, CancellationToken cancellationToken)
        {
            viewLocator.Open(View.Settings);
            return Task.FromResult(true);
        }
    }
}
