using System;

namespace Sidekick.Core.Loggers
{
    public class Log
    {
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
        public LogState State { get; set; }
    }
}
