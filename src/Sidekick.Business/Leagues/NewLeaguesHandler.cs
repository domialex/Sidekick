using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Sidekick.Core.Natives;

namespace Sidekick.Business.Leagues
{
    public class NewLeaguesHandler : INotificationHandler<NewLeagues>
    {
        private readonly INativeNotifications nativeNotifications;
        private readonly IStringLocalizer<NewLeagues> localizer;

        public NewLeaguesHandler(
            INativeNotifications nativeNotifications,
            IStringLocalizer<NewLeagues> localizer)
        {
            this.nativeNotifications = nativeNotifications;
            this.localizer = localizer;
        }

        public Task Handle(NewLeagues notification, CancellationToken cancellationToken)
        {
            nativeNotifications.ShowMessage(localizer["NotificationMessage"]);
            return Task.CompletedTask;
        }
    }
}
