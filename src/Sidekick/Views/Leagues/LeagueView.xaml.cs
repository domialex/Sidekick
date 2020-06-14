using System;
using System.Windows;
using Bindables;

namespace Sidekick.Views.Leagues
{
    /// <summary>
    /// Interaction logic for LeagueView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class LeagueView : BaseOverlay
    {
        public LeagueView(LeagueViewModel leagueViewModel, IServiceProvider serviceProvider)
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

        public LeagueViewModel ViewModel { get; set; }
    }
}
