using System.ComponentModel;
using Sidekick.UI.Leagues.Betrayal;
using Sidekick.UI.Leagues.Delve;

namespace Sidekick.UI.Leagues
{
    public class LeagueViewModel : ILeagueViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LeagueViewModel()
        {
            Betrayal = new BetrayalLeague();
            Delve = new DelveLeague();
        }

        public BetrayalLeague Betrayal { get; private set; }

        public DelveLeague Delve { get; private set; }
    }
}
