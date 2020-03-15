using System;
using System.Windows;
using Bindables;
using Sidekick.UI.Leagues;

namespace Sidekick.Windows.Leagues
{
    /// <summary>
    /// Interaction logic for LeagueView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class LeagueView : BaseWindow
    {
        public LeagueView(ILeagueViewModel leagueViewModel, IServiceProvider serviceProvider)
            : base(serviceProvider,
                  closeOnBlur: true)
        {
            InitializeComponent();
            ViewModel = leagueViewModel;
            DataContext = ViewModel;

            SetWindowPositionPercent(25, 0);
            Show();
            Activate();
        }

        public ILeagueViewModel ViewModel { get; set; }
    }
}
