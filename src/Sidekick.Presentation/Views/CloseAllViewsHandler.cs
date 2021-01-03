using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Presentation.Views.Commands
{
    /// <summary>
    /// Close all opened views
    /// </summary>
    public class CloseAllViewHandler : ICommandHandler<CloseAllViewCommand, bool>
    {
        private readonly IViewLocator viewLocator;

        public CloseAllViewHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public Task<bool> Handle(CloseAllViewCommand request, CancellationToken cancellationToken)
        {
            var result = viewLocator.IsAnyOpened();

            viewLocator.CloseAll();

            return Task.FromResult(result);
        }
    }
}
