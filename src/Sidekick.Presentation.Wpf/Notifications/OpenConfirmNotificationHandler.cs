using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using MediatR;
using Sidekick.Domain.Notifications.Commands;

namespace Sidekick.Notifications
{
    public class OpenConfirmNotificationHandler : ICommandHandler<OpenConfirmNotificationCommand>
    {
        private readonly Dispatcher dispatcher;

        public OpenConfirmNotificationHandler(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public Task<Unit> Handle(OpenConfirmNotificationCommand request, CancellationToken cancellationToken)
        {
            dispatcher.Invoke(async () =>
            {
                var result = AdonisUI.Controls.MessageBox.Show(request.Message, request.Title, AdonisUI.Controls.MessageBoxButton.YesNo);
                if (request.OnYes != null && result == AdonisUI.Controls.MessageBoxResult.Yes)
                {
                    await request.OnYes();
                }
                if (request.OnNo != null && result == AdonisUI.Controls.MessageBoxResult.No)
                {
                    await request.OnNo();
                }
            });

            return Unit.Task;
        }
    }
}
