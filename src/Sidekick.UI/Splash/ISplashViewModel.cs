using System;
using System.ComponentModel;

namespace Sidekick.UI.Splash
{
    public interface ISplashViewModel : INotifyPropertyChanged
    {
        event Action Initialized;
        string StepTitle { get; set; }
        int StepPercentage { get; set; }
        string Title { get; set; }
        int Percentage { get; set; }
    }
}
