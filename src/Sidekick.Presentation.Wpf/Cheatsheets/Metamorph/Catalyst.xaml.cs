using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Metamorph;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Metamorph
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

        public string Image => $"/Cheatsheets/Metamorph/Images/{Model.Image}";
    }
}
