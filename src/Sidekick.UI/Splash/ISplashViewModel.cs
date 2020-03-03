using System;

namespace Sidekick.UI.Splash
{
    public interface ISplashViewModel
    {
        event Action Initialized;

        string StepTitle { get; set; }
        int StepPercentage { get; set; }
        string Title { get; set; }
        int Percentage { get; set; }
    }
}
