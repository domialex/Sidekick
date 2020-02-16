using System.Windows;
using System.Windows.Input;
using Bindables;
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
        public LeagueView(ILeagueViewModel leagueViewModel)
        {
            InitializeComponent();
            ViewModel = leagueViewModel;
            DataContext = ViewModel;

            Show();
            MouseLeftButtonDown += Window_MouseLeftButtonDown;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public ILeagueViewModel ViewModel { get; set; }
    }
}
