using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bindables;
using Sidekick.Core.Natives;
using Sidekick.UI.Leagues;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Leagues
{
    /// <summary>
    /// Interaction logic for LeagueView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class LeagueView : Window, ISidekickView
    {
        private readonly IKeybindEvents keybindEvents;

        public LeagueView(ILeagueViewModel leagueViewModel, IKeybindEvents keybindEvents)
        {
            InitializeComponent();
            ViewModel = leagueViewModel;
            this.keybindEvents = keybindEvents;
            DataContext = ViewModel;

            Show();
            MouseLeftButtonDown += Window_MouseLeftButtonDown;
            keybindEvents.OnCloseWindow += KeybindEvents_OnCloseWindow;
        }

        private Task<bool> KeybindEvents_OnCloseWindow()
        {
            Close();
            keybindEvents.OnCloseWindow -= KeybindEvents_OnCloseWindow;
            return Task.FromResult(true);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public ILeagueViewModel ViewModel { get; set; }
    }
}
