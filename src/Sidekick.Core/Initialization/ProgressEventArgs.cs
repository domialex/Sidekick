using System;

namespace Sidekick.Core.Initialization
{
    public class ProgressEventArgs : EventArgs
    {
        public InitializationSteps Step { get; set; }
        public string Title { get; set; }
        public string StepTitle { get; set; }
        public int StepPercentage { get; set; }
        public int TotalPercentage { get; set; }
    }
}
