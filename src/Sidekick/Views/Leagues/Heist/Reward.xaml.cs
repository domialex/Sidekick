using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Heist
{
    /// <summary>
    /// Interaction logic for Reward.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Reward : UserControl
    {
        public Reward()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public HeistReward Model { get; set; }

        public string Image => $"/Views/Leagues/Heist/Images/{Model.Image}";
    }
}
