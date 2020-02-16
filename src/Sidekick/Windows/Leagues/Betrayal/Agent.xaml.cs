using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.Windows.Leagues.Betrayal
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Agent : UserControl
    {
        public BetrayalAgent Model { get; set; }

        public string Image => $"/Windows/Leagues/Betrayal/Images/{Model.Image}";

        public Agent()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
