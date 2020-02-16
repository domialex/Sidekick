using System.Windows.Controls;
using Sidekick.UI.Leagues;

namespace Sidekick.Windows.Leagues
{
    /// <summary>
    /// Interaction logic for Legend.xaml
    /// </summary>
    public partial class Legend : UserControl
    {
        public Legend()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public string LowColor => RewardValue.NoValue.GetColor();
        public string NormalColor => RewardValue.Low.GetColor();
        public string HighColor => RewardValue.Medium.GetColor();
        public string VeryHighColor => RewardValue.High.GetColor();
    }
}
