using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Metamorph;

namespace Sidekick.Views.Leagues.Metamorph
{
    /// <summary>
    /// Interaction logic for Catalyst.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Catalyst : UserControl
    {
        public Catalyst()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public MetamorphCatalyst Model { get; set; }

        public string Image => $"/Windows/Leagues/Metamorph/Images/{Model.Image}";
    }
}
