using System.ComponentModel;
using Sidekick.UI.Leagues.Betrayal;
using Sidekick.UI.Leagues.Blight;
using Sidekick.UI.Leagues.Delve;
using Sidekick.UI.Leagues.Incursion;
using Sidekick.UI.Leagues.Metamorph;

namespace Sidekick.UI.Leagues
{
    public interface ILeagueViewModel : INotifyPropertyChanged
    {
        BetrayalLeague Betrayal { get; }
        BlightLeague Blight { get; }
        DelveLeague Delve { get; }
        IncursionLeague Incursion { get; }
        MetamorphLeague Metamorph { get; }
    }
}
