using System;

namespace Sidekick.Core.Initialization
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressTypeEnum Type { get; set; }
        public string ServiceName { get; set; }
        public string Message { get; set; }
        public int Percentage { get; set; }
        public int TotalPercentage { get; set; }
    }
}
