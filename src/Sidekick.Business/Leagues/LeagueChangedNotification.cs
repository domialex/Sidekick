using MediatR;

namespace Sidekick.Business.Leagues
{
    /// <summary>
    /// Indicates that the currently selected league has changed
    /// </summary>
    public class LeagueChangedNotification : INotification
    {
    }
}
