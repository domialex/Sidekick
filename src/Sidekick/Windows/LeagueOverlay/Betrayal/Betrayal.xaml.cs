using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.Windows.LeagueOverlay.Betrayal
{
    /// <summary>
    /// Interaction logic for Betrayal.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Betrayal : UserControl
    {
        public Betrayal()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BetrayalLeague League { get; set; }
    }
}