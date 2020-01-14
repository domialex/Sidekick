using System;

namespace Sidekick.Business.Loggers
{
    public class Log
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public LogState State { get; set; }
    }
}
