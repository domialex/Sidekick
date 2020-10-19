using System;

namespace Sidekick.Core.Initialization
{
    public class ErrorEventArgs : EventArgs
    {
        public string StepName { get; set; }
        public string Message { get; set; }
    }
}
