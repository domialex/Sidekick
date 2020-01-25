using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sidekick.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int progressValue;
        public int ProgressValue
        {
            get { return progressValue; }
            set { progressValue = value; OnPropertyChanged(); }
        }

        private string updateStep;
        public string UpdateStep
        {
            get { return updateStep; }
            set { updateStep = value; OnPropertyChanged(); }
        }

        public SplashScreen()
        {
            InitializeComponent();
            DataContext = this;
        }
      
        public void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void UpdateProgress(string updateStep, int progressValue)
        {
            UpdateStep = updateStep;
            ProgressValue = progressValue;
        }
    }
}
