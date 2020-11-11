using System;
using System.Windows;
using Bindables;
using Sidekick.Presentation.Wpf.Views;

namespace Sidekick.Presentation.Wpf.Cheatsheets
{
    /// <summary>
    /// Interaction logic for LeagueView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class LeagueView : BaseOverlay
    {
        public LeagueView(LeagueViewModel leagueViewModel, IServiceProvider serviceProvider)
            : base(Domain.Views.View.League, serviceProvider)
        {
            InitializeComponent();
            ViewModel = leagueViewModel;
            DataContext = ViewModel;

            Show();
            Activate();

            SetTopPercent(0, LocationSource.Begin);
            SetLeftPercent(50, LocationSource.Center);
        }

        public LeagueViewModel ViewModel { get; set; }
    }
}
