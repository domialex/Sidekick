using System;

namespace Sidekick.Core.Initialization
{
    public class ProgressEventArgs : EventArgs
    {
        public string Title { get; set; }
        public int TotalPercentage { get; set; }
        public string StepTitle { get; set; }
        public int StepPercentage { get; set; }
    }
}
