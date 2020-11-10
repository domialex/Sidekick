using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Betrayal
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Agent : UserControl
    {
        public BetrayalAgent Model { get; set; }

        public string Image => $"/Cheatsheets/Betrayal/Images/{Model.Image}";

        public Agent()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
