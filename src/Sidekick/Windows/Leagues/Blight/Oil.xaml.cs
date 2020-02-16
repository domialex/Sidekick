using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Blight;

namespace Sidekick.Windows.Leagues.Blight
{
    /// <summary>
    /// Interaction logic for Oil.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Oil : UserControl
    {
        public Oil()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public BlightOil Model { get; set; }

        public string Image => $"/Windows/Leagues/Blight/Images/{Model.Image}";
    }
}
