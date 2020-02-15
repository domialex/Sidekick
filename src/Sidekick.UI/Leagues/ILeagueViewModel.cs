using System.ComponentModel;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.UI.Leagues
{
    public interface ILeagueViewModel : INotifyPropertyChanged
    {
        BetrayalLeague Betrayal { get; }
    }
}
