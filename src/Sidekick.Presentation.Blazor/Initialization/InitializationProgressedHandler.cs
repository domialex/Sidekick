using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Presentation.Blazor.Initialization
{
    public class InitializationProgressedHandler : INotificationHandler<InitializationProgressed>
    {
        private readonly InitializationViewModel viewModel;

        public InitializationProgressedHandler(
            InitializationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public Task Handle(InitializationProgressed notification, CancellationToken cancellationToken)
        {
            viewModel.Title = notification.Title;
            viewModel.Percentage = notification.Percentage;

            return Task.CompletedTask;
        }
    }
}
