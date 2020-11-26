using MediatR;

namespace Sidekick.Domain.Game.Leagues.Notifications
{
    /// <summary>
    /// Indicates that the currently selected league has changed
    /// </summary>
    public class LeagueChangedNotification : INotification
    {
    }
}
