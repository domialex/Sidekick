using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.Windows.LeagueOverlay.Betrayal
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Agent : UserControl
    {
        public Agent()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BetrayalAgent Item { get; set; }
    }
}
