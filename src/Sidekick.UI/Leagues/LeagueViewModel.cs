using System.ComponentModel;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.UI.Leagues
{
    public class LeagueViewModel : ILeagueViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LeagueViewModel()
        {
            Betrayal = new BetrayalLeague();
        }

        public BetrayalLeague Betrayal { get; private set; }
    }
}
