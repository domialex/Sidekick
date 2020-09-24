using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Heist
{
    /// <summary>
    /// Interaction logic for Reward.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Ally : UserControl
    {
        public Ally()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public HeistAlly Model { get; set; }

        public string Image => $"/Views/Leagues/Heist/Images/{Model.Image}";
    }
}
