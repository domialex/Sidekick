using PropertyChanged;
using Sidekick.Core.Settings;

namespace Sidekick.Views.Leagues
{
    [AddINotifyPropertyChangedInterface]
    public class LeagueViewModel : ILeagueViewModel
    {
        private readonly SidekickSettings settings;

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
