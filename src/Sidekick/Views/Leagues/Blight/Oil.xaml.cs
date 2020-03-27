using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Blight
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
