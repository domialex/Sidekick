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

        public string LowColor => RewardValue.Low.GetColor();
        public string NormalColor => RewardValue.Normal.GetColor();
        public string HighColor => RewardValue.High.GetColor();
        public string VeryHighColor => RewardValue.VeryHigh.GetColor();
    }
}
