using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Heist
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Job : UserControl
    {
        public HeistJob Model { get; set; }

        public string Image => $"/Cheatsheets/Heist/Images/{Model.Image}";

        public Job()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
