using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Presentation.Cheatsheets.Betrayal;
using Sidekick.Presentation.Cheatsheets.Blight;
using Sidekick.Presentation.Cheatsheets.Delve;
using Sidekick.Presentation.Cheatsheets.Heist;
using Sidekick.Presentation.Cheatsheets.Incursion;
using Sidekick.Presentation.Cheatsheets.Metamorph;

namespace Sidekick.Presentation.Wpf.Cheatsheets
{
    public class LeagueViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly ISidekickSettings settings;
        private readonly IMediator mediator;

        public LeagueViewModel(
            ISidekickSettings settings,
            IMediator mediator)
        {
            this.settings = settings;
            this.mediator = mediator;
            Heist = new HeistLeague();
            Betrayal = new BetrayalLeague();
            Blight = new BlightLeague();
            Delve = new DelveLeague();
            Incursion = new IncursionLeague();
            Metamorph = new MetamorphLeague();
        }

        public HeistLeague Heist { get; private set; }
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
                Task.Run(() => mediator.Send(new SaveSettingCommand(nameof(ISidekickSettings.League_SelectedTabIndex), value)));
            }
        }
    }
}
