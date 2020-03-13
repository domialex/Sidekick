using System;

namespace Sidekick.Core.Initialization
{
    public class ErrorEventArgs : EventArgs
    {
        public string ServiceName { get; set; }
        public string Message { get; set; }
    }
}
