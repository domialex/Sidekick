using System.ComponentModel;

namespace Sidekick.Initialization
{
    public class InitializationViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        public string Title { get; set; }
        public int Percentage { get; set; }
        public double ProgressValue => Percentage / 100.0;
    }
}
