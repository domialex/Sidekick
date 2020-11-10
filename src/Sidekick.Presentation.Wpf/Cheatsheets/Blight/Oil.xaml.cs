using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Blight;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Blight
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

        public string Image => $"/Cheatsheets/Blight/Images/{Model.Image}";
    }
}
