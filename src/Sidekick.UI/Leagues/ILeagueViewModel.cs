using System.ComponentModel;
using Sidekick.UI.Leagues.Betrayal;
using Sidekick.UI.Leagues.Delve;

namespace Sidekick.UI.Leagues
{
    public interface ILeagueViewModel : INotifyPropertyChanged
    {
        BetrayalLeague Betrayal { get; }
        DelveLeague Delve { get; }
    }
}
