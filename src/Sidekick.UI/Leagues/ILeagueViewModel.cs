using System;
using System.ComponentModel;

namespace Sidekick.UI.Leagues
{
    public interface ILeagueViewModel : INotifyPropertyChanged
    {
        event Action Initialized;
        string StepTitle { get; set; }
        int StepPercentage { get; set; }
        string Title { get; set; }
        int Percentage { get; set; }
    }
}
