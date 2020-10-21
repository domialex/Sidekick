using System;
using System.ComponentModel;
using Sidekick.Core.Natives;

namespace Sidekick.Initialization
{
    public class InitializationViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly INativeApp nativeApp;

        public InitializationViewModel(
            INativeApp nativeApp)
        {
            this.nativeApp = nativeApp;
        }

        public void Close()
        {
            nativeApp.Shutdown();
        }

        public string Title { get; set; }
        public int Percentage { get; set; }

        public double ProgressValue => Percentage / 100.0;

        public void Complete()
        {
            if (Initialized != null)
            {
                Initialized.Invoke();
            }
        }

        public event Action Initialized;
    }
}
