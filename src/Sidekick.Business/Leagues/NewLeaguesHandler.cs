using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Sidekick.Core.Natives;

namespace Sidekick.Business.Leagues
{
    public class NewLeaguesHandler : INotificationHandler<NewLeaguesNotification>
    {
        private readonly INativeNotifications nativeNotifications;
        private readonly IStringLocalizer<NewLeaguesNotification> localizer;

        public NewLeaguesHandler(
            INativeNotifications nativeNotifications,
            IStringLocalizer<NewLeaguesNotification> localizer)
        {
            this.nativeNotifications = nativeNotifications;
            this.localizer = localizer;
        }

        public Task Handle(NewLeaguesNotification notification, CancellationToken cancellationToken)
        {
            nativeNotifications.ShowMessage(localizer["NotificationMessage"]);
            return Task.CompletedTask;
        }
    }
}
