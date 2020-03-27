using System.ComponentModel;
using Sidekick.Core.Settings;
using Sidekick.Views.Leagues.Betrayal;
using Sidekick.Views.Leagues.Blight;
using Sidekick.Views.Leagues.Delve;
using Sidekick.Views.Leagues.Incursion;
using Sidekick.Views.Leagues.Metamorph;

namespace Sidekick.Views.Leagues
{
    public class LeagueViewModel : INotifyPropertyChanged
    {
        private readonly SidekickSettings settings;

        public event PropertyChangedEventHandler PropertyChanged;

        public LeagueViewModel(
            SidekickSettings settings)
        {
            this.settings = settings;

            Betrayal = new BetrayalLeague();
            Blight = new BlightLeague();
            Delve = new DelveLeague();
            Incursion = new IncursionLeague();
            Metamorph = new MetamorphLeague();
        }

        public BetrayalLeague Betrayal { get; private set; }
        public BlightLeague Blight { get; private set; }
        public DelveLeague Delve { get; private set; }
        public IncursionLeague Incursion { get; private set; }
        public MetamorphLeague Metamorph { get; private set; }

        public int SelectedTabIndex
        {
            get
            {
                return settings.League_SelectedTabIndex;
            }
            set
            {
                settings.League_SelectedTabIndex = value;
                settings.Save();
            }
        }
    }
}
