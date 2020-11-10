using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Heist
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

        public string Image => $"/Cheatsheets/Heist/Images/{Model.Image}";
    }
}
